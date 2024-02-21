using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Cards
{
    internal class ConfigureUserDeckCommand : IRouteCommand
    {
        private readonly IDeckManager _deckManager;
        private readonly IPackageManager _packageManager;
        private readonly List<string> _cardIds;
        private readonly User _user;

        public ConfigureUserDeckCommand(IDeckManager deckManager, IPackageManager packageManager, List<string> cardIds, User user)
        {
            _deckManager = deckManager;
            _packageManager = packageManager;
            _cardIds = cardIds;
            _user = user;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                if(!_packageManager.AreCardsOwnedByUser(_cardIds, _user.Token))
                {
                    throw new NoCardsException();
                }
                _deckManager.ConfigureDeck(_user.Username, _cardIds);
                response = new HttpResponse(StatusCode.Ok);
            }
            catch (NoCardsException)
            {
                response = new HttpResponse(StatusCode.Forbidden);
            }
            catch (NotRequiredAmountOfCardsException)
            {
                response = new HttpResponse(StatusCode.BadRequest);
            }

            return response;
        }
    }
}
