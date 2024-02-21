using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    internal interface IPackageManager
    {
        void AddPackage(List<CardSchema> cards);
        void AcquirePackage(string authToken);
        bool IsPackageAvailable();
        List<CardSchema> GetAllCardsByAuthToken(string authToken);
        bool AreCardsOwnedByUser(List<string> cardIds, string authToken);
    }
}
