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
        public CardType Type { get; set; }
        public ElementType Element { get; set; }

        public Card(string id, string name, float damage, CardType type, ElementType element)
        {
            Id = id;
            Name = name;
            Damage = damage;
            Type = type;    
            Element = element;
        }

        public enum CardType
        {
            Monster,
            Spell
        }

        public enum ElementType
        {
            Water,
            Fire,
            Regular,
        }
    }
}
