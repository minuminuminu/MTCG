using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Tradings
{
    internal class RetrieveCurrentTradingDealsCommand : IRouteCommand
    {
        private readonly ITradingsManager _tradingsManager;
        private readonly User _user;

        public RetrieveCurrentTradingDealsCommand(ITradingsManager tradingsManager, User user)
        {
            _tradingsManager = tradingsManager;
            _user = user;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                var jsonPayload = JsonConvert.SerializeObject(_tradingsManager.GetTradings());
                response = new HttpResponse(StatusCode.Ok, jsonPayload);
            }
            catch (NoTradingDealsException)
            {
                response = new HttpResponse(StatusCode.NoContent);
            }

            return response;
        }
    }
}
