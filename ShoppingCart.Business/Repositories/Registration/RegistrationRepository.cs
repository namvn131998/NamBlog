using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.DataAccess.Data;
using ShoppingCart.DataAccess.Model;
using ShoppingCart.DataAccess.Constants.Enums;
using ShoppingCart.Business.Repositories;

namespace ShoppingCart.Business.Repositories
{
    public class RegistrationRepository : Repository<Registration>, IRegistrationRepository
    {
        private ApplicationDbContext _context;
        public RegistrationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Registration registration)
        {
            var objReg = _context.Registrations.FirstOrDefault(r => r.UserID == registration.UserID);
            if (objReg != null)
            {
                objReg.UserName = registration.UserName;
                objReg.Email = registration.Email;
                objReg.FirstName = registration.FirstName;
                objReg.LastName = registration.LastName;
                objReg.Phone = registration.Phone;
                objReg.Photo = registration.Photo;
            }
        }
        public LoginStatus Login(string username, string password, out Registration registration)
        {
            LoginStatus loginStatus = LoginStatus.InvalidUserName;
            registration = GetUserByName(username);
            if(registration != null)
            {
                if(password == registration.Password)
                {
                    switch(registration.Status)
                    {
                        case ((int)RegistrationStatus.Pending):
                            loginStatus = LoginStatus.Pending;
                            break;
                        case ((int)RegistrationStatus.Disabled):
                            loginStatus = LoginStatus.Disabled;
                            break;
                        default :
                            loginStatus = LoginStatus.Success;
                            break;
                    }
                }
                else
                {
                    loginStatus = LoginStatus.InvalidPassword;
                }
            }
            return loginStatus;
        }
        public Registration GetUserByName(string username="")
        {
            return _context.Registrations.FirstOrDefault(r => r.UserName == username) ?? new Registration();
        }
        public Registration GetUserByID(int UserID)
        {
            return _context.Registrations.FirstOrDefault(r => r.UserID == UserID) ?? new Registration();
        }
    }
}
