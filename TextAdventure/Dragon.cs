namespace TextAdventure
{
    internal class Dragon
    {
        public int Health { get; private set; }

        public Dragon()
        {
            Health = 125;
        }

        public void Attack(Item weapon, int strength, int mood)
        {
            int damage = ((int)weapon - (int)Item.RustyAxe + 1) * 10 + (strength / 10) + (mood / 20);
            Health = Health - damage < 0 ? 0 : Health - damage;
        }

        public DragonResponse Response()
        {
            Random random = new Random();

            DragonResponseType attackType = (DragonResponseType)random.Next((int)DragonResponseType.Flight, (int)DragonResponseType.Miss + 1);
            switch (attackType)
            {
                case DragonResponseType.Flight:
                    return new(10, 10, 10, """
                                           The dragon takes flight, and in the process, sends you into the
                                           wall after knocking you with it's wing
                                           """);
                case DragonResponseType.Fire:
                    return new(25, 15, 20, """
                                           The dragon in a sudden erruption breath fire lighting up the cave.
                                           However in the process you were caught in it.
                                           """);
                case DragonResponseType.Scratch:
                    return new(15, 5, 15, """
                                          Claws at the ready... The dragon starts to attack you with them.
                                          Each shot missing after the next. But in a sudden and unexpected
                                          attack you get caught by one.
                                          """);
                case DragonResponseType.FloorIsLava:
                    return new(30, 20, 20, """
                                           In a burning rage, the dragon musters up all the strength it has and
                                           melts the rocks in the cave, turning them into lava. The floor is now
                                           lava, and your getting burnt.
                                           """);
                case DragonResponseType.Miss:
                    return new(0, 0, 5, "The dragon attempted a sneaky attack, but ultimately failed.");
                default:
                    throw new InvalidDataException($"{attackType}/{(int)attackType} is not a dragon attack type!");
            }
        }
    }

    internal enum DragonResponseType
    {
        Flight,
        Fire,
        Scratch,
        FloorIsLava,
        Miss
    }

    internal record DragonResponse(int HealthLoss, int StrengthLoss, int MoodLoss, string Dialogue);
}
