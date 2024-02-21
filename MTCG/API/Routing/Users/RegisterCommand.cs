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
        private readonly IDeckManager _deckManager;
        private readonly UserCredentials _credentials;

        public RegisterCommand(IUserManager userManager, IDeckManager deckManager, UserCredentials credentials)
        {
            _userManager = userManager;
            _deckManager = deckManager;
            _credentials = credentials;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                _userManager.RegisterUser(_credentials);
                _deckManager.CreateUserDeck(_credentials.Username);
                response = new HttpResponse(StatusCode.Created);
            }
            catch (DuplicateUserException)
            {
                response = new HttpResponse(StatusCode.Conflict);
            }

            return response;
        }
    }
}
