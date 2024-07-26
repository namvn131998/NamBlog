using ShoppingCart.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.DataAccess.Model;
using ShoppingCart.Business.Repositories;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Business.Utilities;

namespace ShoppingCart.Business.Repositories
{
    public interface IBlobRepository
    {
        Task<string> UploadFileToBlobStorage(string filePath, string fileName);
        void DeleteFileBlobStorage(string fileName);
        bool IsExistBlob(string fileName);
    }
}
