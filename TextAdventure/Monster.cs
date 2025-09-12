namespace TextAdventure
{
    internal class Monster
    {
        public const int DragonMaxHealth = 125;
        public const int GrimReaperMaxHealth = 75;
        public const int DevilMaxHealth = 200;

        public MonsterType Type { get; private set; }

        public int Health { get; private set; }

        private Random _rng = new();
        private Dictionary<MonsterEvent, Tuple<string, MonsterPotency>> _events = new()
        {
            { MonsterEvent.DragonFlight, new("""
                                             The dragon takes flight, and in the process, sends you into the
                                             wall after knocking you with it's wing
                                             """,
                                             new(-10, -10, -4)) },
            { MonsterEvent.DragonFire, new("""
                                           The dragon in a sudden erruption breath fire lighting up the cave.
                                           However in the process you were caught in it.
                                           """,
                                           new(-25, -15, -15)) },
            { MonsterEvent.DragonScratch, new("""
                                              Claws at the ready... The dragon starts to attack you with them.
                                              Each shot missing after the next. But in a sudden and unexpected
                                              attack you get caught by one.
                                              """,
                                              new(-15, -5, -10)) },
        };

        public Monster()
        {
            Type = RandomMonster();
            switch (Type)
            {
                case MonsterType.Dragon:
                    Health = DragonMaxHealth;
                    break;
                case MonsterType.GrimReaper:
                    Health = GrimReaperMaxHealth;
                    break;
                case MonsterType.Devil:
                    Health = DevilMaxHealth;
                    break;
            }
        }

        public Tuple<string, MonsterPotency>? Event()
        {
            switch (Type)
            {
                case MonsterType.Dragon:
                    return _events[RandomDragonEvent()];
                case MonsterType.GrimReaper:
                    return _events[RandomGrimReaperEvent()];
                case MonsterType.Devil:
                    return _events[RandomDevilEvent()];
                default:
                    return null;
            }
        }

        private MonsterType RandomMonster() => (MonsterType)_rng.Next((int)MonsterType.Dragon, (int)MonsterType.Devil);
        private MonsterEvent RandomDragonEvent() => (MonsterEvent)_rng.Next((int)MonsterEvent.DragonFlight, (int)MonsterEvent.DragonScratch);
        private MonsterEvent RandomGrimReaperEvent() => (MonsterEvent)_rng.Next((int)MonsterEvent.GrimReaperSythe, (int)MonsterEvent.GrimReaperThreat);
        private MonsterEvent RandomDevilEvent() => (MonsterEvent)_rng.Next((int)MonsterEvent.DevilFloorIsLava, (int)MonsterEvent.DevilThrow);
    }

    internal enum MonsterType
    {
        Dragon,
        GrimReaper,
        Devil
    }

    internal enum MonsterEvent
    {
        DragonFlight,
        DragonFire,
        DragonScratch,

        GrimReaperSythe,
        GrimReaperSkeletonArmy,
        GrimReaperThreat,

        DevilFloorIsLava,
        DevilMeltsWeapon,
        DevilPitchFork,
        DevilThrow
    }

    internal struct MonsterPotency(int healthChange, int strengthChange, int moodChange)
    {
        public int HealthChange { get; private set; } = healthChange;
        public int StrengthChange { get; private set; } = strengthChange;
        public int MoodChange { get; private set; } = moodChange;
    }
}
