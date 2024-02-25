using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    internal interface ITradingsManager
    {
        List<TradingDeal> GetTradings();
        void CreateTradingDeal(TradingDeal tradingDeal);
        void DeleteTradingCommand(string id);
        string GetCardIdFromTradeCommand(string tradeId);
    }
}
