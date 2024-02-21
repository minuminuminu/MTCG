using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    internal class DatabaseCardDao : ICardDao
    {
        private const string CreateCardTableCommand = @"CREATE TABLE IF NOT EXISTS cards (card_id VARCHAR PRIMARY KEY, card_name VARCHAR(255) NOT NULL, damage FLOAT NOT NULL, card_owner VARCHAR(255) DEFAULT NULL, FOREIGN KEY (card_owner) REFERENCES users(token) ON DELETE SET NULL);";
        private const string InsertCardCommand = "INSERT INTO cards(card_id, card_name, damage) VALUES (@card_id, @card_name, @damage)";
        private const string ReassignCardOwnerCommand = "UPDATE cards SET card_owner = @authToken WHERE card_id IN (SELECT card_id FROM package_cards WHERE package_id = @packageId)";
        private const string GetCardsByAuthTokenCommand = "SELECT * FROM cards WHERE card_owner = @authToken";
        private const string CheckCardExistsAndBelongsToUserCommand = "SELECT card_id FROM cards WHERE card_id = @cardId AND card_owner = @authToken;";

        private readonly string _connectionString;

        public DatabaseCardDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public bool AreCardsOwnedByUser(List<string> cardIds, string authToken)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                foreach (string id in cardIds)
                {
                    using (var cmd = new NpgsqlCommand(CheckCardExistsAndBelongsToUserCommand, connection))
                    {
                        cmd.Parameters.AddWithValue("cardId", id);
                        cmd.Parameters.AddWithValue("authToken", authToken);

                        using (var reader = cmd.ExecuteReader())
                        {
                            // If a row is not returned, the card does not exist or does not belong to the user
                            if (!reader.Read())
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public bool InsertCard(CardSchema card)
        {
            // TODO: handle exceptions
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(InsertCardCommand, connection);
            cmd.Parameters.AddWithValue("card_id", card.Id);
            cmd.Parameters.AddWithValue("card_name", card.Name);
            cmd.Parameters.AddWithValue("damage", card.Damage);

            var affectedRows = cmd.ExecuteNonQuery();

            return affectedRows > 0;
        }

        public List<CardSchema> GetCardsByAuthToken(string authToken)
        {
            List<CardSchema> cards = new List<CardSchema>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(GetCardsByAuthTokenCommand, connection);
            cmd.Parameters.AddWithValue("authToken", authToken);
            using var reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                cards.Add(new CardSchema(reader.GetString(reader.GetOrdinal("card_id")), reader.GetString(reader.GetOrdinal("card_name")), reader.GetFloat(reader.GetOrdinal("damage"))));
            }

            return cards;
        }

        public void ReassignCardOwnership(int packageId, string authToken)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(ReassignCardOwnerCommand, connection);
            cmd.Parameters.AddWithValue("authToken", authToken);
            cmd.Parameters.AddWithValue("packageId", packageId);
            cmd.ExecuteNonQuery();
        }

        private void EnsureTables()
        {
            // TODO: handle exceptions
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(CreateCardTableCommand, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
