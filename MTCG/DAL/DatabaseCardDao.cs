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
        private const string CreateCardTableCommand = @"CREATE TABLE IF NOT EXISTS cards (card_id VARCHAR PRIMARY KEY, card_name VARCHAR(255) NOT NULL, damage FLOAT NOT NULL, card_owner VARCHAR(255) DEFAULT NULL, FOREIGN KEY (card_owner) REFERENCES users(username) ON DELETE SET NULL);";
        private const string InsertCardCommand = "INSERT INTO cards(card_id, card_name, damage) VALUES (@card_id, @card_name, @damage)";
        private const string AssignOwnerCommand = "UPDATE cards SET card_owner = @username WHERE card_id = @card_id";

        private readonly string _connectionString;

        public DatabaseCardDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
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
