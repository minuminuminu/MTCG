using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.DAL;
using MTCG.Models;

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
            //var user = new User(credentials.Username, credentials.Password);
            //if (_userDao.InsertUser(user) == false)
            //{
            //    throw new DuplicateUserException();
            //}

            Console.WriteLine("Registered User successfully");
        }
    }
}
