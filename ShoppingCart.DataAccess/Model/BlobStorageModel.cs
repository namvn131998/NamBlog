using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Model
{
    public class BlobStorageModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
