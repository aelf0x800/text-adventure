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

        public void GiveItem(Item item, int amount)
        {
            if (!Inventory.ContainsKey(item))
                Inventory.Add(item, amount);
            else
                Inventory[item] += amount;
        }

        public void TakeItem(Item item, int amount)
        {
            if (Inventory.ContainsKey(item))
            {
                Inventory[item] -= amount;
                if (Inventory[item] < 0)
                    Inventory[item] = 0;
            }
        }

        public void UsePotion(Item potion)
        { }
    }
}
