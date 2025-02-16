using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.DataAccess.Helper;
using System.Collections;
using ShoppingCart.DataAccess.Model;

namespace ShoppingCart.Business.Utilities
{
    public static class SessionUtilities
    {
        public static string SessionCurrentUserkey = "CurrentUser";
        public static string SessionCart = "Cart";
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
        public static List<Cart>? GetCart<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value != null)
            {
                if (value.TrimStart().StartsWith("["))
                {
                    // Deserialize as list
                    var carts = JsonConvert.DeserializeObject<List<Cart>>(value);
                    return carts;
                }
                else
                {
                    // Deserialize single object and convert to list
                    var cart = JsonConvert.DeserializeObject<Cart>(value);
                    var carts = new List<Cart> { cart };
                    return carts;
                }
            }
            else
            {
                return null;
            }
        }
        public static void SetInt(this ISession session, string key, int value)
        {
            session.SetInt32(key, value);
        }

        public static int? GetInt(this ISession session, string key)
        {
            return session.GetInt32(key);
        }

        public static void RemoveKey(this ISession session, string key)
        {
            session.Remove(key);
        }
        #region user
        public static bool IsLogged(this ISession session)
        {
            return session.Get(SessionCurrentUserkey) != null;
        }
        public static LoggedUser CurrentUser(this ISession session)
        {
            return IsLogged(session) ? session.Get<LoggedUser>(SessionCurrentUserkey) : null;
        }
        public static List<Cart> GetListCart (this ISession session)
        {
            return session.GetCart<List<Cart>>(SessionCart);
        }
        #endregion
    }
}

