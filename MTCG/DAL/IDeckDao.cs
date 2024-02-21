using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    internal interface IDeckDao
    {
        void CreateUserDeckEntry(string username);
        List<CardSchema> GetDeckCardByUsername(string username);
        void ConfigureDeck(List<string> cardIds, string username);
    }
}
