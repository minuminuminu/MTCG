using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.API.Routing;
using MTCG.API.Routing.Users;
using MTCG.API.Routing.Packages;
using MTCG.BLL;
using MTCG.HttpServer.Request;
using MTCG.Models;
using Newtonsoft.Json;

namespace MTCG.HttpServer.Routing
{
    internal class Router
    {
        private readonly IUserManager _userManager;
        private readonly IPackageManager _packageManager;
        private readonly IdentityProvider _identityProvider;
        private readonly IdRouteParser _routeParser;

        public Router(IUserManager userManager, IPackageManager packageManager)
        {
            _userManager = userManager;
            _identityProvider = new IdentityProvider(userManager);
            _routeParser = new IdRouteParser();
            _packageManager = packageManager;
        }

        public IRouteCommand? Resolve(HttpRequest request)
        {
            try
            {
                return request switch
                {
                    { Method: Request.HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_userManager, Deserialize<UserCredentials>(request.Payload)),
                    { Method: Request.HttpMethod.Post, ResourcePath: "/sessions" } => new LoginCommand(_userManager, Deserialize<UserCredentials>(request.Payload)),
                    { Method: Request.HttpMethod.Post, ResourcePath: "/packages" } => new CreatePackageCommand(_packageManager, Deserialize<List<CardSchema>>(request.Payload), GetIdentity(request).Token),
                    { Method: Request.HttpMethod.Post, ResourcePath: "/transactions/packages" } => new AcquirePackageCommand(_packageManager, GetIdentity(request)),

                    _ => null
                }; ;
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
