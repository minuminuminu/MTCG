using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    internal interface IScoreManager
    {
        UserStats GetUserStats(string username);
        List<UserStats> GetScoreboard();
        void CreateStatsEntryForNewUser(string username);
    }
}
