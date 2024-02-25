using MTCG.DAL;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    internal class TradingsManager : ITradingsManager
    {
        private readonly ITradingsDao _tradingsDao;

        public TradingsManager(ITradingsDao tradingsDao)
        {
            _tradingsDao = tradingsDao;
        }

        public void DeleteTradingCommand(string id)
        {
            if (!_tradingsDao.DeleteTradingCommand(id)) throw new CardNotFoundException();
        }

        public string GetCardIdFromTradeCommand(string tradeId)
        {
            var tradingDeal = _tradingsDao.GetTradingDealCommand(tradeId);
            if (tradingDeal != null)
            {
                return tradingDeal.CardToTrade;
            }
            else
            {
                throw new CardNotFoundException();
            }
        }


        public void CreateTradingDeal(TradingDeal tradingDeal)
        {
            try
            {
                _tradingsDao.CreateTradingDeal(tradingDeal);
            }
            catch (Npgsql.PostgresException ex)
            {
                if (ex.SqlState == "23505") // duplicate pkey violation code
                {
                    throw new DuplicateTradingDealException();
                }
            }
        }

        public List<TradingDeal> GetTradings()
        {
            List<TradingDeal> tradings = _tradingsDao.GetTradings();

            if(tradings.Count == 0)
            {
                throw new NoTradingDealsException();
            }

            return tradings;
        }
    }
}
