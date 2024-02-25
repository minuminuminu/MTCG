using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    internal class DatabaseTradingsDao : ITradingsDao
    {
        private const string CreateTradingsTableCommand = @"CREATE TABLE IF NOT EXISTS tradings (id VARCHAR PRIMARY KEY, card_to_trade_id VARCHAR REFERENCES cards(card_id), type VARCHAR, minimum_damage FLOAT);";
        private const string GetAllTradingDealsCommand = "SELECT * FROM tradings;";
        private const string CreateTradingDealCommand = "INSERT INTO tradings (id, card_to_trade_id, type, minimum_damage) VALUES (@id, @cardToTradeId, @type, @minimumDamage);";
        private const string DeleteTradingDealCommand = "DELETE FROM tradings WHERE id = @id;";
        private const string GetTradingDealByIdCommand = "SELECT * FROM tradings WHERE id = @id;";

        private readonly string _connectionString;

        public DatabaseTradingsDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public TradingDeal? GetTradingDealCommand(string tradeId)
        {
            TradingDeal? trade = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(GetTradingDealByIdCommand, connection))
                {
                    cmd.Parameters.AddWithValue("id", tradeId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            trade = new TradingDeal(
                                reader.GetString(reader.GetOrdinal("id")),
                                reader.GetString(reader.GetOrdinal("card_to_trade_id")),
                                reader.GetString(reader.GetOrdinal("type")),
                                reader.GetFloat(reader.GetOrdinal("minimum_damage"))
                                );
                        }
                    }
                }
            }

            return trade;
        }


        public bool DeleteTradingCommand(string id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(DeleteTradingDealCommand, connection))
                {
                    cmd.Parameters.AddWithValue("id", id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }

        public void CreateTradingDeal(TradingDeal tradingDeal)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(CreateTradingDealCommand, connection);
            cmd.Parameters.AddWithValue("id", tradingDeal.Id);
            cmd.Parameters.AddWithValue("cardToTradeId", tradingDeal.CardToTrade);
            cmd.Parameters.AddWithValue("type", tradingDeal.Type);
            cmd.Parameters.AddWithValue("minimumDamage", tradingDeal.MinimumDamage);

            cmd.ExecuteNonQuery();
        }

        public List<TradingDeal> GetTradings()
        {
            List<TradingDeal> tradings = new List<TradingDeal>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(GetAllTradingDealsCommand, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader.GetString(reader.GetOrdinal("id"));
                            string cardToTradeId = reader.GetString(reader.GetOrdinal("card_to_trade_id"));
                            string type = reader.GetString(reader.GetOrdinal("type"));
                            float minimumDamage = reader.GetFloat(reader.GetOrdinal("minimum_damage"));

                            tradings.Add(new TradingDeal(id, cardToTradeId, type, minimumDamage));
                        }
                    }
                }
            }

            return tradings;
        }

        private void EnsureTables()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(CreateTradingsTableCommand, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
