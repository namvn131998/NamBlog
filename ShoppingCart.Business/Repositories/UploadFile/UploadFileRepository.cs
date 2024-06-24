using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.DataAccess.Data;
using ShoppingCart.Models;
using ShoppingCart.DataAccess.Model;
using ShoppingCart.Business.Utilities;

namespace ShoppingCart.Business.Repositories
{
    public class UploadFileRepository : Repository<UploadFile>, IUploadFileRepository
    {
        private ApplicationDbContext _context;
        public UploadFileRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public List<UploadFile> GetListMediaByProductMediaIDs(string mediaids, string fileName)
        {
            List<int> numbersList = Commons.ConvertStringToList(mediaids);
            var uploadfiles = _context.UploadFiles.Where(u => numbersList.Contains(u.MediaID) && u.FileName == fileName).ToList();
            return uploadfiles;
        }
        public string GetThumbnailFromUploadFile(string mediaIDs)
        {
            List<int> numbersList = Commons.ConvertStringToList(mediaIDs);
            var uploadfile = _context.UploadFiles.FirstOrDefault(u => numbersList.Contains(u.MediaID)) ?? new UploadFile();
            return uploadfile.Thumbnail;
        }
        public List<UploadFile> GetThumbnailsFromUploadFile(string mediaIDs)
        {
            List<int> numbersList = Commons.ConvertStringToList(mediaIDs);
            var uploadfile = _context.UploadFiles.Where(u => numbersList.Contains(u.MediaID)).ToList();
            
            return uploadfile;
        }
    }
}
