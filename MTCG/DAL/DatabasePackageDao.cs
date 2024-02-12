using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    internal class DatabasePackageDao : IPackageDao
    {
        private const string CreatePackageTableCommand = @"CREATE TABLE IF NOT EXISTS packages (package_id SERIAL PRIMARY KEY);";
        private const string CreatePackageCardsTableCommand = @"CREATE TABLE IF NOT EXISTS package_cards (
            package_id INT NOT NULL,
            card_id VARCHAR NOT NULL,
            FOREIGN KEY (package_id) REFERENCES packages(package_id),
            FOREIGN KEY (card_id) REFERENCES cards(card_id),
            PRIMARY KEY (package_id, card_id)
        );";
        private const string CreatePackageCommand = @"INSERT INTO packages DEFAULT VALUES RETURNING package_id";
        private const string InsertPackageCommand = "INSERT INTO package_cards(package_id, card_id) VALUES (@package_id, @card_id)";
        private const string SelectAllPackagesCommand = "SELECT * FROM packages";
        private const string DeletePackageCommand = "DELETE FROM packages WHERE package_id=@package_id";
        private const string DeletePackageCardsRelationCommand = "DELETE FROM package_cards WHERE package_id=@package_id";

        private readonly string _connectionString;

        public DatabasePackageDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        private int CreatePackageRow()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(CreatePackageCommand, connection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool InsertPackage(List<string> cardIds)
        {
            int newPackageId = CreatePackageRow();

            // TODO: handle exceptions
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            foreach (var cardId in cardIds)
            {
                using var cmd = new NpgsqlCommand(InsertPackageCommand, connection);
                cmd.Parameters.AddWithValue("package_id", newPackageId);
                cmd.Parameters.AddWithValue("card_id", cardId);
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        private void DeletePackage(int packageId)
        {
            // delete card - package relationship
            // delete package entry
        }

        private void EnsureTables()
        {
            // TODO: handle exceptions
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(CreatePackageTableCommand, connection);
            cmd.ExecuteNonQuery();

            cmd.CommandText = CreatePackageCardsTableCommand;
            cmd.ExecuteNonQuery();
        }
    }
}
