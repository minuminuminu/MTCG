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
        public List<Card> Stack { get; set; }
        public Card[] Deck = new Card[4];
        public string Token => $"{Username}-mtcgToken";

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Coins = 20;
            Stack = new List<Card>();
        }

        public User(string username, string password, int coins)
        {
            Username = username;
            Password = password;
            Coins = coins;
            Stack = new List<Card>();
        }
    }
}
