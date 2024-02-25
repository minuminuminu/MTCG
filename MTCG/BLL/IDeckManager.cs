using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    internal interface IDeckManager
    {
        void CreateUserDeck(string username);
        List<CardSchema> GetCardsInDeck(string username);
        void ConfigureDeck(string username, List<string> cards);
        void IsCardInDeck(string cardId);
    }
}
