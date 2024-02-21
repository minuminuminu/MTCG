using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    internal class DatabaseDeckDao : IDeckDao
    {
        private const string CreateDecksTableCommand = @"CREATE TABLE IF NOT EXISTS decks (
            deck_id SERIAL PRIMARY KEY,
            user_id VARCHAR REFERENCES users(username),
            card1_id VARCHAR DEFAULT NULL,
            card2_id VARCHAR DEFAULT NULL,
            card3_id VARCHAR DEFAULT NULL,
            card4_id VARCHAR DEFAULT NULL,
            CONSTRAINT unique_card_per_deck UNIQUE (deck_id, card1_id, card2_id, card3_id, card4_id)
        );
        ";
        private const string InsertDefaultUserDeckCommand = "INSERT INTO decks(user_id) VALUES (@username)";
        private const string GetUserDeckByUsernameCommand = "SELECT * FROM decks WHERE user_id=@username";
        private const string GetDeckCardsByUsernameCommand = @"SELECT c.card_id, c.card_name, c.damage
            FROM cards c
            JOIN decks d ON c.card_id IN (d.card1_id, d.card2_id, d.card3_id, d.card4_id)
            WHERE d.user_id = @username;
        ";
        private const string ConfigureDeckWithCardIdsCommand = @"UPDATE decks SET card1_id=@card1_id, card2_id=@card2_id, card3_id=@card3_id, card4_id=@card4_id WHERE user_id=@username;";

        private readonly string _connectionString;

        public DatabaseDeckDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public void CreateUserDeckEntry(string username)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(InsertDefaultUserDeckCommand, connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.ExecuteNonQuery();
        }

        public void ConfigureDeck(List<string> cardIds, string username)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(ConfigureDeckWithCardIdsCommand, connection);
            cmd.Parameters.AddWithValue("username", username);

            for(int i = 0; i < cardIds.Count; i++)
            {
                cmd.Parameters.AddWithValue($"card{i + 1}_id", cardIds[i]);
            }
            cmd.ExecuteNonQuery();
        }

        public List<CardSchema> GetDeckCardByUsername(string username)
        {
            List<CardSchema> deckCards = new List<CardSchema>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(GetDeckCardsByUsernameCommand, connection))
                {
                    cmd.Parameters.AddWithValue("username", username);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            deckCards.Add((new CardSchema(reader.GetString(reader.GetOrdinal("card_id")), reader.GetString(reader.GetOrdinal("card_name")), reader.GetFloat(reader.GetOrdinal("damage")))));
                        }
                    }
                }
            }

            return deckCards;
        }


        private void EnsureTables()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(CreateDecksTableCommand, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
