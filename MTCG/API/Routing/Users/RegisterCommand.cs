using MTCG.BLL;
using MTCG.HttpServer.Routing;
using MTCG.Models;
using MTCG.HttpServer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Users
{
    internal class RegisterCommand : IRouteCommand
    {
        private readonly IUserManager _userManager;
        private readonly UserCredentials _credentials;

        public RegisterCommand(IUserManager userManager, UserCredentials credentials)
        {
            _userManager = userManager;
            _credentials = credentials;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                _userManager.RegisterUser(_credentials);
                response = new HttpResponse(StatusCode.Created, "Hello there!");
                Console.WriteLine("Testing Payload");
                Console.WriteLine("Username: " + _credentials.Username);
                Console.WriteLine("Password: " + _credentials.Password);
            }
            catch (DuplicateUserException)
            {
                response = new HttpResponse(StatusCode.Conflict);
            }

            return response;
        }
    }
}
