using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Users
{
    internal class LoginCommand : IRouteCommand
    {
        private readonly IUserManager _userManager;
        private readonly UserCredentials _credentials;

        public LoginCommand(IUserManager userManager, UserCredentials credentials)
        {
            _credentials = credentials;
            _userManager = userManager;
        }

        public HttpResponse Execute()
        {
            User? user;
            try
            {
                user = _userManager.LoginUser(_credentials);
            }
            catch (UserNotFoundException)
            {
                user = null;
            }

            HttpResponse response;
            if(user == null)
            {
                response = new HttpResponse(StatusCode.Unauthorized);
            }
            else
            {
                response = new HttpResponse(StatusCode.Ok, user.Token);
            }

            return response;
        }
    }
}
