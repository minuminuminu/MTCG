using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.DAL
{
    internal interface IUserDao
    {
        User? GetUserByAuthToken(string authToken);
        User? GetUserByUsername(string username);
        bool UpdateUserData(string username, UserData userData);
        User? GetUserByCredentials(string username, string password);
        bool InsertUser(User user);
        void WithdrawCoins(int amount, string username);
    }
}
