using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.DataAccess.Model;

namespace ShoppingCart.DataAccess.Helper
{
    public class LoggedUser
    {
        public LoggedUser()
        {

        }
        public LoggedUser(Registration reg)
        {
            this.UserId = reg.UserID;
            this.Username = reg.UserName;
            this.FirstName = reg.FirstName;
            this.LastName = reg.LastName;
            this.Phone = reg.Phone;
            this.Photo = reg.Photo;
        }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int? UserType { get; set; }
        public string Photo { get; set; }
    }
}

