using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MTCG.Models
{
    internal class UserCredentials
    {
        public string Username { get; set;}
        public string Password { get; set; }

        public UserCredentials(string username, string password)
        {
            Username = username;   
            Password = password;
        }
    }
}
