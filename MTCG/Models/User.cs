using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class User
    {
        public string Username { get; set; }
        public string Password { get; set; }    
        public int Coins { get; set; }
        //public List<CardSchema> Stack { get; set; }
        public string Token => $"{Username}-mtcgToken";
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Coins = 20;
            //Stack = new List<CardSchema>();
            Name = string.Empty;
            Bio = string.Empty;
            Image = string.Empty;
        }

        public User(string username, string password, int coins, string name, string bio, string image)
        {
            Username = username;
            Password = password;
            Coins = coins;
            //Stack = new List<CardSchema>();
            Name = name;
            Bio = bio;
            Image = image;
        }
    }
}
