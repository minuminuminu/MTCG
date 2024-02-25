using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    internal interface ITradingsDao
    {
        List<TradingDeal> GetTradings();
        void CreateTradingDeal(TradingDeal tradingDeal);
        bool DeleteTradingCommand(string id);
        TradingDeal? GetTradingDealCommand(string tradeId);
    }
}
