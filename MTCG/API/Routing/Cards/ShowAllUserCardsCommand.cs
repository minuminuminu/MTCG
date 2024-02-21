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

namespace MTCG.API.Routing.Cards
{
    internal class ShowAllUserCardsCommand : IRouteCommand
    {
        IPackageManager _packageManager;
        string _authToken;

        public ShowAllUserCardsCommand(IPackageManager packageManager, string authToken)
        {
            _packageManager = packageManager;
            _authToken = authToken;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                List<CardSchema> cards = _packageManager.GetAllCardsByAuthToken(_authToken);
                var jsonPayload = JsonConvert.SerializeObject(cards);

                response = new HttpResponse(StatusCode.Ok, jsonPayload);
            }
            catch (NoCardsException)
            {
                response = new HttpResponse(StatusCode.NoContent);
            }

            return response;
        }
    }
}
