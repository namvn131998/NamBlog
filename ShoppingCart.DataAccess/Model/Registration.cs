using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Model
{
    public class Registration
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Newpassword { get; set; }
        public string? Confirmpassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public string? Photo { get; set; }   
        public int UserType { get; set; }   
    }
}
