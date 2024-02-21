using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Users
{
    internal class UpdateUserDataCommand : IRouteCommand
    {
        private readonly IUserManager _userManager;
        private readonly string _username;
        private readonly User _user;
        private readonly UserData _data;

        public UpdateUserDataCommand(string username, User user, IUserManager userManager, UserData data)
        {
            _userManager = userManager;
            _username = username;
            _user = user;
            _data = data;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                if (_user.Token != "admin-mtcgToken" && _user.Username != _username)
                {
                    response = new HttpResponse(StatusCode.Unauthorized);
                    return response;
                }

                _userManager.UpdateUserData(_username, _data);
                response = new HttpResponse(StatusCode.Created);
            }
            catch (UserNotFoundException)
            {
                response = new HttpResponse(StatusCode.NotFound);
            }

            return response;
        }
    }
}
