using MTCG.BLL;
using MTCG.Models;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MTCG.API.Routing.Users
{
    internal class RetrieveUserDataCommand : IRouteCommand
    {
        private readonly string _username;
        private readonly User _user;
        private readonly IUserManager _userManager;

        public RetrieveUserDataCommand(string username, User user, IUserManager userManager)
        {
            _username = username;
            _user = user;
            _userManager = userManager;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                if(_user.Token != "admin-mtcgToken" && _user.Username != _username)
                {
                    response = new HttpResponse(StatusCode.Unauthorized);
                    return response;
                }

                User requestedUser = _userManager.GetUserByUsername(_username);

                UserData userData = new UserData(requestedUser.Name, requestedUser.Bio, requestedUser.Image);
                var jsonPayload = JsonConvert.SerializeObject(userData);

                response = new HttpResponse(StatusCode.Ok, jsonPayload);
            }
            catch (UserNotFoundException)
            {
                response = new HttpResponse(StatusCode.NotFound);
            }

            return response;
        }
    }
}
