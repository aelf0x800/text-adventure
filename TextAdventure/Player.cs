using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    internal class Player(string name)
    {
        public const int MaxHealth = 150;
        public const int MaxStrength = 100;
        public const int MaxMood = 100;

        public string Name = name;
        public int Health = MaxHealth;
        public int Strength = MaxStrength;
        public int Mood = MaxMood;
        public Dictionary<Item, int> Inventory = new Dictionary<Item, int>();
    }
}
