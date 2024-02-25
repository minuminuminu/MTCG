using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Stats
{
    internal class ShowScoreboardCommand : IRouteCommand
    {
        private readonly IScoreManager _scoreManager;
        private readonly User _user;

        public ShowScoreboardCommand(IScoreManager scoreManager, User user)
        {
            _scoreManager = scoreManager;
            _user = user; // für access token check, ob vorhanden od nd
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            List<UserStats> scoreboard = _scoreManager.GetScoreboard();
            var jsonPayload = JsonConvert.SerializeObject(scoreboard);
            response = new HttpResponse(StatusCode.Ok, jsonPayload);

            return response;
        }
    }
}
