using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Tradings
{
    internal class CarryOutTradeCommand : IRouteCommand
    {
        private readonly string _tradeId;
        private readonly ITradingsManager _tradingManager;
        private readonly IDeckManager _deckManager;
        private readonly IPackageManager _packageManager;
        private readonly User _user;
        private readonly string _cardId;

        public CarryOutTradeCommand(string tradeId, ITradingsManager tradingManager, IDeckManager deckManager, IPackageManager packageManager, User user, string cardId)
        {
            _tradeId = tradeId;
            _tradingManager = tradingManager;
            _deckManager = deckManager;
            _packageManager = packageManager;
            _user = user;
            _cardId = cardId;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                string id = _tradingManager.GetCardIdFromTradeCommand(_tradeId);

                _packageManager.IsCardOwnedByUser(id, _user.Token);
                _deckManager.IsCardInDeck(id);
            }
            catch (CardNotAvailableException)
            {
                response = new HttpResponse(StatusCode.Forbidden);
            }
            catch (CardNotFoundException)
            {
                response = new HttpResponse(StatusCode.NotFound);
            }

            return response;
        }
    }
}
