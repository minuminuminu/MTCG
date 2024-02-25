using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    internal class DatabaseScoreDao : IScoreDao
    {
        private const string CreateScoreboardCommand = @"CREATE TABLE IF NOT EXISTS scoreboard (username VARCHAR UNIQUE REFERENCES users(username), elo INT DEFAULT 0, losses INT DEFAULT 0, wins INT DEFAULT 0);";
        private const string CreateUserStatsEntry = "INSERT INTO scoreboard (username) VALUES (@username);";
        private const string GetUserStatsByUsernameCommand = "SELECT sb.*, u.name FROM scoreboard sb INNER JOIN users u ON sb.username = u.username WHERE sb.username = @username;";
        private const string GetEntireScoreboardCommand = "SELECT sb.*, u.name FROM scoreboard sb INNER JOIN users u ON sb.username = u.username ORDER BY sb.elo DESC;";

        private readonly string _connectionString;

        public DatabaseScoreDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public void CreateStatsEntryForNewUser(string username)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(CreateUserStatsEntry, connection);
            cmd.Parameters.AddWithValue("username", username);

            cmd.ExecuteNonQuery();
        }

        public UserStats? GetUserStats(string username)
        {
            UserStats? stats = null;

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(GetUserStatsByUsernameCommand, connection);
            cmd.Parameters.AddWithValue("username", username);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string name = reader.GetString(reader.GetOrdinal("name"));
                int elo = reader.GetInt32(reader.GetOrdinal("elo"));
                int losses = reader.GetInt32(reader.GetOrdinal("losses"));
                int wins = reader.GetInt32(reader.GetOrdinal("wins"));

                stats = new UserStats(name, elo, wins, losses);
            }

            return stats;
        }

        public List<UserStats> GetScoreboard()
        {
            List<UserStats> scoreboard = new List<UserStats>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(GetEntireScoreboardCommand, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader.IsDBNull(reader.GetOrdinal("name")) ? "Anonymous" : reader.GetString(reader.GetOrdinal("name"));
                            int elo = reader.GetInt32(reader.GetOrdinal("elo"));
                            int losses = reader.GetInt32(reader.GetOrdinal("losses"));
                            int wins = reader.GetInt32(reader.GetOrdinal("wins"));

                            scoreboard.Add(new UserStats(name, elo, wins, losses));
                        }
                    }
                }
            }

            return scoreboard;
        }


        private void EnsureTables()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(CreateScoreboardCommand, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
