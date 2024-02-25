using MTCG.DAL;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    internal class ScoreManager : IScoreManager
    {
        private readonly IScoreDao _scoreDao;

        public ScoreManager(IScoreDao scoreDao)
        {
            _scoreDao = scoreDao;
        }

        public UserStats GetUserStats(string username)
        {
            return _scoreDao.GetUserStats(username) ?? throw new UserNotFoundException();
        }

        public List<UserStats> GetScoreboard()
        {
            return _scoreDao.GetScoreboard();
        }

        public void CreateStatsEntryForNewUser(string username)
        {
            _scoreDao.CreateStatsEntryForNewUser(username);
        }
    }
}
