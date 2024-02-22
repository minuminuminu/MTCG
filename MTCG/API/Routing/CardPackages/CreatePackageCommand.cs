using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Response;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.BLL;

namespace MTCG.API.Routing.CardPackages
{
    internal class CreatePackageCommand : IRouteCommand
    {
        string authToken;
        List<CardSchema> cards;
        private readonly IPackageManager _packageManager;

        public CreatePackageCommand(IPackageManager packageManager, List<CardSchema> package, string adminToken)
        {
            _packageManager = packageManager;
            authToken = adminToken;
            cards = package;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            if (authToken != "admin-mtcgToken")
            {
                response = new HttpResponse(StatusCode.Forbidden);
                return response;
            }

            try
            {
                _packageManager.AddPackage(cards);
                response = new HttpResponse(StatusCode.Created);
            }
            catch (DuplicateCardException)
            {
                response = new HttpResponse(StatusCode.Conflict);
            }

            return response;
        }
    }
}
