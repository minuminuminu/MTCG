using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    internal interface IUserManager
    {
        User LoginUser(UserCredentials credentials);
        void RegisterUser(UserCredentials credentials);
        User GetUserByAuthToken(string authToken);
        void WithdrawCoinsForPackage(string authToken);
    }
}
