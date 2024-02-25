using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    internal interface IScoreDao
    {
        UserStats? GetUserStats(string username);
        List<UserStats> GetScoreboard();
        void CreateStatsEntryForNewUser(string username);
    }
}
