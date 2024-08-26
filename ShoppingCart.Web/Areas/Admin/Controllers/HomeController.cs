using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DataAccess.ViewModels;
using ShoppingCart.DataAccess.Model;
using ShoppingCart.DataAccess.Constants.Enums;
using ShoppingCart.Business.Utilities;
using ShoppingCart.DataAccess.Helper;
using ShoppingCart.Business.Repositories;
using Microsoft.CodeAnalysis;

namespace ShoppingCart.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IBlobRepository _blobRepository;
        public HomeController(IUnitOfWork unitOfWork, IBlobRepository blobRepository)
        {
            _unitOfWork = unitOfWork;
            _blobRepository = blobRepository;   
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AdminLogin(string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }
        [HttpPost]
        public  IActionResult AdminLogin(LoginViewModel log)
        {
            string message = "InvalidUserName";
            Registration registration = new Registration();
            LoginStatus loginStatus = _unitOfWork.RegistrationRepository.Login(log.Username, log.Password, out registration);
            if(registration != null)
            {
                switch (loginStatus)
                {
                    case LoginStatus.Pending:
                        message = "Your account is pending";
                        break;
                    case LoginStatus.Disabled:
                        message = "Your account is disabled";
                        break;
                    case LoginStatus.InvalidUserName:
                        message = "InvalidUserName";
                        ModelState.AddModelError("UserName", message);
                        break;
                    case LoginStatus.InvalidPassword:
                        message = "InvalidPassword";
                        ModelState.AddModelError("Password", message);
                        break;
                    default:
                        message = "LoginSuccess";
                        break;
                }
            }
            if(loginStatus == LoginStatus.Success)
            {
                var user = new LoggedUser(registration);
                HttpContext.Session.Set(SessionUtilities.SessionCurrentUserkey, user);
                return RedirectToAction("Index", "Category");
            }
            else
            {
                return View();
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Set(SessionUtilities.SessionCurrentUserkey, "");
            return RedirectToAction("AdminLogin", "Home");
        }
        [HttpGet]
        public IActionResult Register()
        {
            var model = new Registration();
            return View(model);
        }
        [HttpPost]
        public IActionResult Register(Registration reg)
        {
            if (ModelState.IsValid)
            {
                if (reg.UserID == 0)
                {
                    _unitOfWork.RegistrationRepository.Add(reg);
                }
                _unitOfWork.Save();
            }
            return Json(new {message="OK"});
        }
        [HttpGet]
        public IActionResult Profile(int UserID)
        {
            var model = _unitOfWork.RegistrationRepository.GetUserByID(UserID);
            return View(model);
        }
        [HttpGet]
        public IActionResult Changepassword(int UserID)
        {
            ViewBag.UserID = UserID;
            return View();
        }
        [HttpPost]
        public IActionResult Changepassword(int UserID = 0, string Password = "",string Newpassword = "", string Confirmpassword = "")
        {
            var user = _unitOfWork.RegistrationRepository.GetUserByID(UserID);
            if (user.Password == Password)
            {
                user.Password = Newpassword;
                _unitOfWork.Save();
                return Json(new { result = "OK" });
            }
            else
            {
                return Json(new { result = "Fail" });
            }
        }
        [HttpPost]
        public IActionResult UpdateUser(Registration reg)
        {
            _unitOfWork.RegistrationRepository.Update(reg);
            HttpContext.Session.Set(SessionUtilities.SessionCurrentUserkey, reg);
            _unitOfWork.Save();
            var model = _unitOfWork.RegistrationRepository.GetUserByID(reg.UserID);
            return RedirectToAction("Profile", model);
        }
        [HttpPost]
        public async Task<Result> UploadUserAvatar( int userID, int productID, IFormFile files, int MediaTypeID)
        { 
            var result = new Result();
            var folderHost = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var userFolder = $"UserID-{userID}/Avatar";
            var pathFolderMedia = Path.Combine("Upload");
            var fullpathFolderMedia = Path.Combine(folderHost, pathFolderMedia);
            if (!Directory.Exists(fullpathFolderMedia))
            {
                Directory.CreateDirectory(fullpathFolderMedia);
            }
            var LocalPathFile = Path.Combine(fullpathFolderMedia, files.FileName);
            using (var stream = new FileStream(LocalPathFile, FileMode.Create))
            {
                await files.CopyToAsync(stream);
            }
            string PathfileName = String.Format("{0}/{1}", userFolder, files.FileName);
            var isExist = _blobRepository.IsExistBlob(PathfileName);
            if (!isExist)
            {
                var uploadBlob = _blobRepository.UploadFileToBlobStorage(LocalPathFile, PathfileName);
                // get rootFolder of image
                try
                {
                    // Check if file exists with its full path
                    if (System.IO.File.Exists((Path.Combine(fullpathFolderMedia, files.FileName))))
                    {
                        // If file found, delete it
                        System.IO.File.Delete(Path.Combine(fullpathFolderMedia, files.FileName));
                    }
                }
                catch (IOException ioExp)
                {
                }
                result.value = "OK";
                result.fileName = files.FileName;
                result.UrlThumbnail = uploadBlob.Result;
                return result;
            }
            else
            {
                result.fileName = files.FileName;
                result.value = "Fail";
                return result;
            }
        }
    }
}
