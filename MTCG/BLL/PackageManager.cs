using MTCG.DAL;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    internal class PackageManager : IPackageManager
    {
        private readonly IPackageDao _packageDao;
        private readonly ICardDao _cardDao;

        public PackageManager(IPackageDao packageDao, ICardDao cardDao)
        {
            _packageDao = packageDao;
            _cardDao = cardDao;
        }

        public void AddPackage(List<CardSchema> cards)
        {
            List<string> cardIds = new List<string>();

            try
            {
                foreach(var card in cards)
                {
                    _cardDao.InsertCard(card);
                    cardIds.Add(card.Id);
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                if (ex.SqlState == "23505") // duplicate pkey violation code
                {
                    throw new DuplicateCardException();
                }
                else
                {
                    // other exceptions
                    throw;
                }
            }

            _packageDao.InsertPackage(cardIds);
        }

        public bool AreCardsOwnedByUser(List<string> cardIds, string authToken)
        {
            return _cardDao.AreCardsOwnedByUser(cardIds, authToken);
        }

        public bool IsPackageAvailable()
        {
            return _packageDao.IsPackageAvailable();
        }

        public List<CardSchema> GetAllCardsByAuthToken(string authToken)
        {
            List<CardSchema> cards = _cardDao.GetCardsByAuthToken(authToken);

            if(cards.Count == 0)
            {
                throw new NoCardsException();
            }

            return cards;
        }

        public void AcquirePackage(string authToken)
        {
            try
            {
                int packageId = _packageDao.GetOldestPackageId();

                _cardDao.ReassignCardOwnership(packageId, authToken);
                _packageDao.DeletePackage(packageId);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
