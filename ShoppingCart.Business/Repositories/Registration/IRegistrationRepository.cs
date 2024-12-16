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
    public interface IRegistrationRepository : IRepository<Registration>
    {
        void Update(Registration registration);
        LoginStatus Login(string username, string password, out Registration registration);
        Registration GetUserByName(string username);
        Registration GetUserByID(int UserID);
    }
}
