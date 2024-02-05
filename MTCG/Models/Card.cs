using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class Card
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float Damage { get; set; }

        public Card(string id, string name, float damage)
        {
            Id = id;
            Name = name;
            Damage = damage;
        }
    }
}
