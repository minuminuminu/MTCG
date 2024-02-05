using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.API.Routing;
using MTCG.API.Routing.Users;
using MTCG.BLL;
using MTCG.HttpServer.Request;
using MTCG.Models;
using Newtonsoft.Json;

namespace MTCG.HttpServer.Routing
{
    internal class Router
    {
        private readonly IUserManager _userManager;
        private readonly IdentityProvider _identityProvider;
        private readonly IdRouteParser _routeParser;

        public Router(IUserManager userManager)
        {
            _userManager = userManager;
            _identityProvider = new IdentityProvider(userManager);
            _routeParser = new IdRouteParser();
        }

        public IRouteCommand? Resolve(HttpRequest request)
        {
            try
            {
                return request switch
                {
                    { Method: Request.HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_userManager, Deserialize<UserCredentials>(request.Payload)),

                    _ => null
                };
            } catch(InvalidDataException)
            {
                return null;
            }
        }

        private T Deserialize<T>(string? body) where T : class
        {
            var data = body is not null ? JsonConvert.DeserializeObject<T>(body) : null;
            return data ?? throw new InvalidDataException();
        }

        private User GetIdentity(HttpRequest request)
        {
            return _identityProvider.GetIdentityForRequest(request) ?? throw new RouteNotAuthenticatedException();
        }
    }
}
