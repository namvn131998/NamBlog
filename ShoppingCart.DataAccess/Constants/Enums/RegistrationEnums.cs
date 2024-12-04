using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ShoppingCart.DataAccess.Constants.Enums
{
    public enum RegistrationStatus
    {
        Pending = 0,
        Activated = 1,
        Disabled = 2
    }
    public enum UserType
    {
        [Description("Customer")]
        Customer = 0,
        [Description("Registrants")]
        User = 1,
        [Description("SuperAdmin")]
        Superadmin = 2,
        [Description("Admin")]
        Admin = 3
    }
    public enum LoginStatus
    {
        InvalidUserName,
        InvalidPassword,
        Success,
        Pending,
        Disabled
    }
}
