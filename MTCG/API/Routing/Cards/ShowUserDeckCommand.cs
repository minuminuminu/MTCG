using MTCG.BLL;
using MTCG.Models;
using MTCG.HttpServer.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.HttpServer.Response;
using Newtonsoft.Json;

namespace MTCG.API.Routing.Cards
{
    internal class ShowUserDeckCommand : IRouteCommand
    {
        private readonly IDeckManager _deckManager;
        private readonly string _username;

        public ShowUserDeckCommand(IDeckManager deckManager, string username)
        {
            _deckManager = deckManager;
            _username = username;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                List<CardSchema> cards = _deckManager.GetCardsInDeck(_username);
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
