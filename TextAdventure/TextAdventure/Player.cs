namespace TextAdventure
{
    internal class Player
    {
        public const int MaxHealth = 120;
        public const int MaxStrength = 100;
        public const int MaxMood = 100;

        public string Name { get; private set; }
        public int Health { get; private set; }
        public int Strength { get; private set; }
        public int Mood { get; private set; }
        public Dictionary<Item, int> Inventory { get; private set; }

        public Player(string name)
        {
            Name = name;
            Health = MaxHealth;
            Strength = MaxStrength;
            Mood = MaxMood;
            Inventory = new();

            Inventory.Add(Item.RustyAxe, 1);
            Inventory.Add(Item.Carrot, 2);
            Inventory.Add(Item.HealPotion, 1);
        }

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
                Inventory[item] -= Inventory[item] < 0 ? 0 : amount;
        }

        public void UseConsumeable(Item consumeable)
        {
            switch (consumeable)
            {
                case Item.HealPotion:
                    Health = Health + 20 > MaxHealth ? MaxHealth : Health + 20;
                    break;
                case Item.StrengthPotion:
                    Strength = Strength + 20 > MaxStrength ? MaxStrength : Strength + 20;
                    break;
                case Item.Beer:
                    Mood = Mood + 20 > MaxMood ? MaxMood : Mood + 20;
                    break;
                default:
                    if (consumeable < Item.HealPotion && consumeable > Item.Carrot)
                        throw new InvalidDataException($"{consumeable} is not a consumeable");
                    else
                    {
                        Health = Health + 10 > MaxHealth ? MaxHealth : Health + 10;
                        Strength = Strength + 10 > MaxStrength ? MaxStrength : Strength + 10;
                    }
                    break;
            }
        }

        public void UpdateStats(DragonResponse dragonResponse)
        {
            Health = Health - dragonResponse.HealthLoss < 0 ? 0 : Health - dragonResponse.HealthLoss;
            Strength = Strength - dragonResponse.StrengthLoss < 0 ? 0 : Strength - dragonResponse.StrengthLoss;
            Mood = Mood - dragonResponse.MoodLoss < 0 ? 0 : Mood - dragonResponse.MoodLoss;
        }
    }
}
