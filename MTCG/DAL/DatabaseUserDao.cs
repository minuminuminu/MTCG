﻿using Npgsql;
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
        private const string CreateUserTableCommand = @"CREATE TABLE IF NOT EXISTS users (username varchar PRIMARY KEY, password varchar, coins integer, token varchar UNIQUE, name varchar DEFAULT NULL, bio varchar DEFAULT NULL, image varchar DEFAULT NULL);";
        private const string SelectAllUsersCommand = @"SELECT * FROM users";
        private const string SelectUserByCredentialsCommand = "SELECT * FROM users WHERE username=@username AND password=@password";
        private const string InsertUserCommand = @"INSERT INTO users(username, password, coins, token) VALUES (@username, @password, @coins, @token)";
        private const string WithdrawCoinsCommand = "UPDATE users SET coins=coins-@amount WHERE username=@username";
        private const string UpdateUserDataCommand = @"UPDATE users SET name=@name, bio=@bio, image=@image WHERE username=@username;";


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
        public User? GetUserByUsername(string username)
        {
            return GetAllUsers().SingleOrDefault(u => u.Username == username);
        }

        public bool UpdateUserData(string username, UserData userData)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(UpdateUserDataCommand, connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("name", userData.Name);
            cmd.Parameters.AddWithValue("bio", userData.Bio);
            cmd.Parameters.AddWithValue("image", userData.Image);

            var affectedRows = cmd.ExecuteNonQuery();
            return affectedRows > 0;
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

        public void WithdrawCoins(int amount, string username)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(WithdrawCoinsCommand, connection);
            cmd.Parameters.AddWithValue("amount", amount);
            cmd.Parameters.AddWithValue("username", username);
            var affectedRows = cmd.ExecuteNonQuery();
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
            var name = Convert.ToString(record["name"])!;
            var bio = Convert.ToString(record["bio"])!;
            var image = Convert.ToString(record["image"])!;

            return new User(username, password, coins, name, bio, image);
        }
    }
}
