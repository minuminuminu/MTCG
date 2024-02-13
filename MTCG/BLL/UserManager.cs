using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.DAL;
using MTCG.Models;
using Npgsql;

namespace MTCG.BLL
{
    internal class UserManager : IUserManager
    {
        private readonly IUserDao _userDao;

        public UserManager(IUserDao userDao)
        {
            _userDao = userDao;
        }

        public User GetUserByAuthToken(string authToken)
        {
            return _userDao.GetUserByAuthToken(authToken) ?? throw new UserNotFoundException();
        }

        public User LoginUser(UserCredentials credentials)
        {
            return _userDao.GetUserByCredentials(credentials.Username, credentials.Password) ?? throw new UserNotFoundException();
        }

        public void RegisterUser(UserCredentials credentials)
        {
            try
            {
                var user = new User(credentials.Username, credentials.Password);
                _userDao.InsertUser(user);
            }
            catch (Npgsql.PostgresException ex)
            {
                if (ex.SqlState == "23505") // duplicate pkey violation code
                {
                    throw new DuplicateUserException();
                }
                else
                {
                    // other exceptions
                    throw;
                }
            }
        }

        public void WithdrawCoinsForPackage(string authToken)
        {
            User? user = GetUserByAuthToken(authToken);

            if(user.Coins < 5)
            {
                throw new NotEnoughCoinsException();
            }

            _userDao.WithdrawCoins(5, user.Username);
        }
    }
}
