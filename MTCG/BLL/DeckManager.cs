using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.DAL;
using MTCG.Models;

namespace MTCG.BLL
{
    internal class DeckManager : IDeckManager
    {
        private readonly IDeckDao _deckDao;

        public DeckManager(IDeckDao deckDao)
        {
            _deckDao = deckDao;
        }

        public List<CardSchema> GetCardsInDeck(string username)
        {
            List<CardSchema> cards = _deckDao.GetDeckCardByUsername(username);

            if(cards.Count == 0)
            {
                throw new NoCardsException();
            }

            return cards;
        }

        public void ConfigureDeck(string username, List<string> cards)
        {
            if(cards.Count != 4)
            {
                throw new NotRequiredAmountOfCardsException();
            }

            _deckDao.ConfigureDeck(cards, username);
        }

        public void IsCardInDeck(string cardId)
        {
            if (_deckDao.IsCardInDeck(cardId)) throw new CardNotAvailableException();
        }

        public void CreateUserDeck(string username)
        {
            _deckDao.CreateUserDeckEntry(username);
        }
    }
}
