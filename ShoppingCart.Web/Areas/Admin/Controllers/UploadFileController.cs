using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Business.Repositories;
using ShoppingCart.DataAccess.ViewModels;
using ShoppingCart.DataAccess.Model;
using ShoppingCart.Models.Category;
using ShoppingCart.DataAccess.Constants.Enums;
using ShoppingCart.Business.Utilities;
using ShoppingCart.DataAccess.Helper;
using X.PagedList;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Text.Json;
using Microsoft.CodeAnalysis;

namespace ShoppingCart.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UploadFileController : Controller
    {

        private IUnitOfWork _unitOfWork;
        private IBlobRepository _blobRepository;
        private HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
        public UploadFileController(IUnitOfWork unitOfWork, IBlobRepository blobRepository)
        {
            _unitOfWork = unitOfWork;
            _blobRepository = blobRepository;
        }
        [HttpGet]
        public IActionResult ShowUploadFile(string isProduct = "false" , int productID = 0)
        {
            ViewBag.isProduct = isProduct;
            ViewBag.productID = productID;
            return PartialView("_ShowUploadFile");
        }
        [HttpGet]
        public IActionResult ShowListMedia(int mediaid)
        {
            var host = HttpContext.Request.Host.ToString();
            host = Commons.GetPathImage(host);
            var Media = _unitOfWork.UploadFileRepository.GetT(x => x.MediaID == mediaid);
            ViewBag.Media = Media;
            ViewBag.Host = host;
            return PartialView("_ShowListMedia");
        }
        [HttpPost]
        public IActionResult DeleteMedia(int mediaid, int userID, string FileName = "", int productID = 0)
        {
            var uploadfile = _unitOfWork.UploadFileRepository.GetT(x => x.MediaID == mediaid);
            // split Thumbnail to get pathFile.
            string pathFile = uploadfile.Thumbnail.Split("namvucontainer/")[1];
            if (uploadfile == null)
            {
                return NotFound();
            }
            _blobRepository.DeleteFileBlobStorage(pathFile);
            _unitOfWork.UploadFileRepository.Delete(uploadfile);
            _unitOfWork.Save();
            return Json(new { result = "OK" });
        }
        [HttpPost]
        public async Task<IActionResult> AddListProductThumbnail(List<IFormFile> files, int userID, int productID, int MediaTypeID)
        {
            int mediaID = 0;
            var mediaids = _unitOfWork.ProductRepository.GetT(p => p.Id == productID).MediaIds; 
            var folderHost = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var userFolder = $"UserID-{userID}/ProductID-{productID}";
            foreach (IFormFile file in files)
            {
                if (file.Length > 0)
                {
                    var extensionFile = Path.GetExtension(file.FileName);
                    if (extensionFile == ".jpg" || extensionFile == ".png")
                    {
                        var result = await SaveProductThumbnail(file, folderHost, userID, userFolder, MediaTypeID,productID);
                        mediaID = result.id;
                        if(result.value == "OK")
                        {
                            mediaids = mediaids + "," + mediaID;
                        }
                        else
                        {
                            return Json(result);
                        }
                    }
                }
            }
            mediaids = mediaids.Trim(',');
            _unitOfWork.ProductRepository.UpdateMediaID(productID, mediaids);
            _unitOfWork.Save();
            return Json(new Result() {value = "OK"});
        }
        private async Task<Result> SaveProductThumbnail(IFormFile file, string folderHost, int userID, string userFolder, int MediaTypeID, int productID)
        {
            var result = new Result();
            var pathFolderMedia = Path.Combine("Upload");
            var fullpathFolderMedia = Path.Combine(folderHost, pathFolderMedia);
            if (!Directory.Exists(fullpathFolderMedia))
            {
                Directory.CreateDirectory(fullpathFolderMedia);
            }
            var LocalPathFile = Path.Combine(fullpathFolderMedia, file.FileName);
            using (var stream = new FileStream(LocalPathFile, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            string PathfileName = String.Format("{0}/{1}", userFolder, file.FileName);
            var isExist = _blobRepository.IsExistBlob(PathfileName);
            if (!isExist)
            {
                var uploadBlob = _blobRepository.UploadFileToBlobStorage(LocalPathFile, PathfileName);
                var uploadfile = new UploadFile
                {
                    FileName = file.FileName,
                    Thumbnail = uploadBlob.Result,
                    UploadDate = DateTime.Now,
                    MediaTypeID = MediaTypeID,
                    UserID = userID
                };
                // get rootFolder of image
                try
                {
                    // Check if file exists with its full path
                    if (System.IO.File.Exists((Path.Combine(fullpathFolderMedia, file.FileName))))
                    {
                        // If file found, delete it
                        System.IO.File.Delete(Path.Combine(fullpathFolderMedia, file.FileName));
                    }
                }
                catch (IOException ioExp)
                {
                }
                _unitOfWork.UploadFileRepository.Add(uploadfile);
                _unitOfWork.Save();
                result.id = uploadfile.MediaID;
                result.value = "OK";
                result.fileName = file.FileName;
                return result;
            }
            else
            {
                result.fileName = file.FileName;
                result.value = "Fail";
                return result;
            }
        }
    }
}

