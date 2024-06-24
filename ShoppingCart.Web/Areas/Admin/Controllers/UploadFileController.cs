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
        public IActionResult ShowUploadFile(string isProduct, int productID)
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
            if (uploadfile == null)
            {
                return NotFound();
            }
            _blobRepository.DeleteFileBlobStorage(FileName);
            _unitOfWork.UploadFileRepository.Delete(uploadfile);
            _unitOfWork.Save();
            // get rootFolder of image
            var folderHost = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var pathFolderMedia = Path.Combine("Upload");
            var rootFolder = Path.Combine(folderHost, pathFolderMedia);
            try
            {
                // Check if file exists with its full path
                if (System.IO.File.Exists((Path.Combine(rootFolder, FileName))))
                {
                    // If file found, delete it
                    System.IO.File.Delete(Path.Combine(rootFolder, FileName));
                }
            }
            catch (IOException ioExp)
            {
            }
            return Json(new { result = "OK" });
        }
        [HttpPost]
        public async Task<IActionResult> AddListThumbnail(List<IFormFile> files, int userID, int productID, int MediaTypeID , int UploadTypeID)
        {
            int mediaID = 0;
            var mediaids = _unitOfWork.ProductRepository.GetT(p => p.Id == productID).MediaIds; 
            string userFolder = "";
            var folderHost = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            switch (UploadTypeID)
            {
                case (int)UploadType.User:
                    userFolder = $"UserID-{userID}/Avatar";
                    
                    break;
                case (int)UploadType.Product:
                    userFolder = $"UserID-{userID}/ProductID-{productID}";
                    break;
            }
            foreach (IFormFile file in files)
            {
                if (file.Length > 0)
                {
                    var extensionFile = Path.GetExtension(file.FileName);
                    if (extensionFile == ".jpg" || extensionFile == ".png")
                    {
                        var result = await SaveThumbnail(file, folderHost, userID, userFolder, MediaTypeID, UploadTypeID,productID);
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
        private async Task<Result> SaveThumbnail(IFormFile file, string folderHost, int userID, string userFolder, int MediaTypeID, int UploadTypeID, int productID)
        {
            var result = new Result();
            var pathFolderMedia = Path.Combine("Upload");
            var fullpathFolderMedia = Path.Combine(folderHost, pathFolderMedia);
            if (!Directory.Exists(fullpathFolderMedia))
            {
                Directory.CreateDirectory(fullpathFolderMedia);
            }
            var PathFile = Path.Combine(fullpathFolderMedia, file.FileName);
            using (var stream = new FileStream(PathFile, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            string fileName = String.Format("{0}/{1}", userFolder, file.FileName);
            var uploadBlob = _blobRepository.UploadFileToBlobStorage(PathFile, fileName);
            var isExist = CheckExistMedia(file.FileName, productID);
            if (!isExist)
            {
                var uploadfile = new UploadFile
                {
                    FileName = file.FileName,
                    Thumbnail = uploadBlob.Result,
                    UploadDate = DateTime.Now,
                    MediaTypeID = MediaTypeID,
                    UploadTypeID = UploadTypeID,
                    UserID = userID
                };
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
        public bool CheckExistMedia(string FileName, int productID)
        {
            var isExist = true;
            var mediaids = _unitOfWork.ProductRepository.GetT(x => x.Id == productID).MediaIds;

            var uploadFile = _unitOfWork.UploadFileRepository.GetListMediaByProductMediaIDs(mediaids, FileName);

            if (uploadFile.Count == 0) 
            {
                isExist = false;
            }
            return isExist;
        }
    }
}

