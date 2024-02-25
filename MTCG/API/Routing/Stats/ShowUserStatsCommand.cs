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

namespace MTCG.API.Routing.Stats
{
    internal class ShowUserStatsCommand : IRouteCommand
    {
        private readonly IScoreManager _scoreManager;
        private readonly User _user;

        public ShowUserStatsCommand(IScoreManager scoreManager, User user)
        {
            _scoreManager = scoreManager;
            _user = user;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                UserStats stats = _scoreManager.GetUserStats(_user.Username);
                var jsonPayload = JsonConvert.SerializeObject(stats);
                response = new HttpResponse(StatusCode.Ok, jsonPayload);
            }
            catch (UserNotFoundException)
            {
                response = new HttpResponse(StatusCode.NotFound);
            }

            return response;
        }
    }
}
