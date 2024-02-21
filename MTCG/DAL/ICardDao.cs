using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    internal interface ICardDao
    {
        bool InsertCard(CardSchema card);
        void ReassignCardOwnership(int packageId, string authToken);
        List<CardSchema> GetCardsByAuthToken(string authToken);
        bool AreCardsOwnedByUser(List<string> cardIds, string authToken);
    }
}
