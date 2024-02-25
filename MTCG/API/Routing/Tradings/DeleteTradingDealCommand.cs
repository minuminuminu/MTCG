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
    internal class DeleteTradingDealCommand : IRouteCommand
    {
        private readonly ITradingsManager _tradingManager;
        private readonly IPackageManager _packageManager;
        private readonly string _tradingDealId;
        private readonly User _user;

        public DeleteTradingDealCommand(ITradingsManager tradingManager, IPackageManager packageManager, string tradingDealId, User user)
        {
            _tradingManager = tradingManager;
            _packageManager = packageManager;
            _tradingDealId = tradingDealId;
            _user = user;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                _packageManager.IsCardOwnedByUser(_tradingManager.GetCardIdFromTradeCommand(_tradingDealId), _user.Token);
                _tradingManager.DeleteTradingCommand(_tradingDealId);
                response = new HttpResponse(StatusCode.Ok);
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
