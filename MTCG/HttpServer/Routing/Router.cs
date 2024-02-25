using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.API.Routing;
using MTCG.API.Routing.Users;
using MTCG.API.Routing.CardPackages;
using MTCG.API.Routing.Cards;
using MTCG.API.Routing.Stats;
using MTCG.BLL;
using MTCG.HttpServer.Request;
using MTCG.Models;
using Newtonsoft.Json;
using MTCG.API.Routing.Tradings;

namespace MTCG.HttpServer.Routing
{
    internal class Router
    {
        private readonly IUserManager _userManager;
        private readonly IPackageManager _packageManager;
        private readonly IDeckManager _deckManager;
        private readonly IScoreManager _scoreManager;
        private readonly ITradingsManager _tradingsManager;
        private readonly IdentityProvider _identityProvider;
        private readonly IdRouteParser _routeParser;

        public Router(IUserManager userManager, IDeckManager deckManager, IPackageManager packageManager, IScoreManager scoreManager, ITradingsManager tradingsManager)
        {
            _userManager = userManager;
            _identityProvider = new IdentityProvider(userManager);
            _deckManager = deckManager;
            _routeParser = new IdRouteParser();
            _packageManager = packageManager;
            _scoreManager = scoreManager;
            _tradingsManager = tradingsManager;
        }

        public IRouteCommand? Resolve(HttpRequest request)
        {
            var isMatch = (string resourcePath, string patternPath) => _routeParser.IsMatch(resourcePath, "/" + patternPath + "/{id}");
            var parseId = (string resourcePath, string patternPath) => _routeParser.ParseParameters(resourcePath, "/" + patternPath + "/{id}")["id"];

            try
            {
                return request switch
                {
                    { Method: Request.HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_userManager, _deckManager, _scoreManager, Deserialize<UserCredentials>(request.Payload)),
                    { Method: Request.HttpMethod.Post, ResourcePath: "/sessions" } => new LoginCommand(_userManager, Deserialize<UserCredentials>(request.Payload)),
                    { Method: Request.HttpMethod.Post, ResourcePath: "/packages" } => new CreatePackageCommand(_packageManager, Deserialize<List<CardSchema>>(request.Payload), GetIdentity(request).Token),
                    { Method: Request.HttpMethod.Post, ResourcePath: "/transactions/packages" } => new AcquirePackageCommand(_packageManager, _userManager, GetIdentity(request).Token),
                    { Method: Request.HttpMethod.Get, ResourcePath: "/cards" } => new ShowAllUserCardsCommand(_packageManager, GetIdentity(request).Token),
                    { Method: Request.HttpMethod.Get, ResourcePath: "/deck" } => new ShowUserDeckCommand(_deckManager, GetIdentity(request).Username),
                    { Method: Request.HttpMethod.Put, ResourcePath: "/deck" } => new ConfigureUserDeckCommand(_deckManager, _packageManager, Deserialize<List<string>>(request.Payload), GetIdentity(request)),
                    { Method: Request.HttpMethod.Get, ResourcePath: var path } when isMatch(path, "users") => new RetrieveUserDataCommand(parseId(path, "users"), GetIdentity(request), _userManager),
                    { Method: Request.HttpMethod.Put, ResourcePath: var path } when isMatch(path, "users") => new UpdateUserDataCommand(parseId(path, "users"), GetIdentity(request), _userManager, Deserialize<UserData>(request.Payload)),
                    { Method: Request.HttpMethod.Get, ResourcePath: "/stats" } => new ShowUserStatsCommand(_scoreManager, GetIdentity(request)),
                    { Method: Request.HttpMethod.Get, ResourcePath: "/scoreboard" } => new ShowScoreboardCommand(_scoreManager, GetIdentity(request)),
                    { Method: Request.HttpMethod.Get, ResourcePath: "/tradings" } => new RetrieveCurrentTradingDealsCommand(_tradingsManager, GetIdentity(request)),
                    { Method: Request.HttpMethod.Post, ResourcePath: "/tradings" } => new CreateTradingDealCommand(_tradingsManager, _packageManager, _deckManager, GetIdentity(request), Deserialize<TradingDeal>(request.Payload)),
                    { Method: Request.HttpMethod.Delete, ResourcePath: var path } when isMatch(path, "tradings") => new DeleteTradingDealCommand(_tradingsManager, _packageManager, parseId(path, "tradings"), GetIdentity(request)),
                    { Method: Request.HttpMethod.Post, ResourcePath: var path } when isMatch(path, "tradings") => new CarryOutTradeCommand(parseId(path, "tradings"), _tradingsManager, _deckManager, _packageManager, GetIdentity(request)),

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
