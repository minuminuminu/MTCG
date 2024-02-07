using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.DAL
{
    internal class DatabaseUserDao : IUserDao
    {
        private const string CreateUserTableCommand = @"CREATE TABLE IF NOT EXISTS users (username varchar PRIMARY KEY, password varchar, coins integer, token varchar);";
        private const string SelectAllUsersCommand = @"SELECT * FROM users";
        private const string SelectUserByCredentialsCommand = "SELECT * FROM users WHERE username=@username AND password=@password";
        private const string InsertUserCommand = @"INSERT INTO users(username, password, coins, token) VALUES (@username, @password, @coins, @token)";


        private readonly string _connectionString;

        public DatabaseUserDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public User? GetUserByAuthToken(string authToken)
        {
            return GetAllUsers().SingleOrDefault(u => u.Token == authToken);
        }

        public User? GetUserByCredentials(string username, string password)
        {
            // TODO: handle exceptions
            User? user = null;

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectUserByCredentialsCommand, connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            // take the first row, if any
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user = ReadUser(reader);
            }

            return user;
        }

        public bool InsertUser(User user)
        {
            // TODO: handle exceptions
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(InsertUserCommand, connection);
            cmd.Parameters.AddWithValue("username", user.Username);
            cmd.Parameters.AddWithValue("password", user.Password);
            cmd.Parameters.AddWithValue("coins", user.Coins);
            cmd.Parameters.AddWithValue("token", user.Token);
            var affectedRows = cmd.ExecuteNonQuery();

            return affectedRows > 0;
        }

        private void EnsureTables()
        {
            // TODO: handle exceptions
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateUserTableCommand, connection);
            cmd.ExecuteNonQuery();
        }

        private IEnumerable<User> GetAllUsers()
        {
            // TODO: handle exceptions
            var users = new List<User>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectAllUsersCommand, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var user = ReadUser(reader);
                users.Add(user);
            }

            return users;
        }

        private User ReadUser(IDataRecord record)
        {
            var username = Convert.ToString(record["username"])!;
            var password = Convert.ToString(record["password"])!;
            var coins = Convert.ToInt16(record["coins"])!;

            return new User(username, password, coins);
        }
    }
}
