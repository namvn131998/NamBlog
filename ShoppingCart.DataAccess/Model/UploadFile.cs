﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Model
{
    public class UploadFile
    {
        [Key]
        public int MediaID { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string Thumbnail { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public DateTime UploadDate { get; set; }
        public int MediaTypeID { get; set; }
        public int UploadTypeID { get; set; }
    }
}
