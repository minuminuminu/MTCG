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
    internal class CreateTradingDealCommand : IRouteCommand
    {
        private readonly ITradingsManager _tradingManager;
        private readonly IPackageManager _packageManager;
        private readonly IDeckManager _deckManager;
        private readonly User _user;
        private readonly TradingDeal _tradingDeal;

        public CreateTradingDealCommand(ITradingsManager tradingManager, IPackageManager packageManager, IDeckManager deckManager, User user, TradingDeal tradingDeal)
        {
            _tradingManager = tradingManager;
            _packageManager = packageManager;
            _deckManager = deckManager;
            _user = user;
            _tradingDeal = tradingDeal;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                _packageManager.IsCardOwnedByUser(_tradingDeal.CardToTrade, _user.Token);
                _deckManager.IsCardInDeck(_tradingDeal.CardToTrade);
                _tradingManager.CreateTradingDeal(_tradingDeal);
                response = new HttpResponse(StatusCode.Created);
            }
            catch (CardNotAvailableException)
            {
                response = new HttpResponse(StatusCode.Forbidden);
            }
            catch (DuplicateTradingDealException)
            {
                response = new HttpResponse(StatusCode.Conflict);
            }

            return response;
        }
    }
}
