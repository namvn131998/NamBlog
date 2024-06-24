using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.DataAccess.Model;

namespace ShoppingCart.Business.Repositories
{
    public interface IUploadFileRepository : IRepository<UploadFile>
    {
        string GetThumbnailFromUploadFile(string mediaIDs);
        List<UploadFile> GetThumbnailsFromUploadFile(string mediaIDs);
        List<UploadFile> GetListMediaByProductMediaIDs(string mediaids, string FileName);
    }
}
