namespace TextAdventure
{
    internal class Game
    {
        private Random _random;
        private Player _player;

        public Game(string name)
        {
            _random = new();
            _player = new(name);
        }

        public void Intro()
        {
            Console.Clear();

            MessageBox(Art.Village1AM);
            DialogueBox("It's 1 am...");

            MessageBox(Art.MineSounds);
            DialogueBox("""
                        Sounds are comming from the village's local gemstone mines.
                        Sounds unlike any heard before...
                        """);
                

            MessageBox(Art.VillageSleeping);
            DialogueBox("""
                        The village remains in a deep sleep, undisturbed by the sounds,
                        while you grow curious...
                        """);

            MessageBox(Art.MineEnter);
            DialogueBox("The curiosity takes over, and you begin to prepare your \"investigation\"...");
            DialogueBox("""
                        Supplies packed, a torch in one hand, and an old rusty axe in the other, 
                        you begin your journey into the mines.
                        """);
        }

        public void Play()
        {
            while (_player.Health > 0)
            {
                GameEvent gameEvent = (GameEvent)_random.Next((int)GameEvent.Loot, (int)GameEvent.Dragon + 1);
                switch (gameEvent)
                {
                    case GameEvent.Loot:
                        LootFound();
                        break;
                    case GameEvent.Wizard:
                        WizardFound();
                        break;
                    case GameEvent.Dragon:
                        DragonFound();
                        break;
                }
            }
        }

        private void LootFound() 
        {
            Tuple<Item, int> itemA = new(RandomConsumableMisc(), _random.Next(1, 3));
            Tuple<Item, int> itemB = new(RandomConsumableMisc(), _random.Next(1, 3));

            string promptString = $"""  
                                  You found some loot...
                                  
                                  a) {itemA.Item2}x {itemA.Item1}
                                  b) {itemB.Item2}x {itemB.Item1}
                                  
                                  But you can only take one of them.
                                  So which one will it be?
                                  """;

            char itemChoice = PromptBox(promptString, [ 'a', 'b' ]);
            if (itemChoice == 'a')
                _player.GiveItem(itemA.Item1, itemA.Item2);
            else
                _player.GiveItem(itemB.Item1, itemB.Item2);
        }

        private void DragonFound() 
        {
            MessageBox("You found a dragon!");
            MessageBox(Art.Dragon);

            Dragon dragon = new();
            while (dragon.Health > 0 && _player.Health > 0)
            {
                char choice = PromptBox("What do you do?\n\na) Attack\nb) Use consumables", [ 'a', 'b' ]);
                if (choice == 'a')
                {
                    Item? weapon = ItemPickerMenu(Item.RustyAxe, Item.RubyBroadSword, "Weapons");
                    if (weapon != null)
                        dragon.Attack(weapon.Value, _player.Strength, _player.Mood);

                    DragonResponse dragonResponse = dragon.Response();
                    
                    DialogueBox(dragonResponse.Dialogue);
                    _player.UpdateStats(dragonResponse);

                    MessageBox($"""
                               You now have {_player.Health} health, {_player.Strength} strength, and
                               {_player.Mood} mood.
                               """);
                }
                else
                {
                    Item? consumeable = ItemPickerMenu(Item.HealPotion, Item.Carrot, "Consumeables");
                    if (consumeable != null)
                    {
                        _player.UseConsumeable(consumeable.Value);
                        _player.TakeItem(consumeable.Value, 1);

                        MessageBox($"""
                                   You used a {consumeable}, and now have {_player.Health} health,
                                   {_player.Strength} strength, and {_player.Mood} mood.
                                   """);
                    }
                }
            }

            if (_player.Health > 0)
            {
                Item randomWeapon = RandomWeapon();
                Item randomPotion = RandomPotion();

                _player.GiveItem(randomWeapon, 1);
                _player.GiveItem(randomPotion, 2);

                DialogueBox($"For your victory you got a {randomWeapon} and 2x {randomPotion}");
            }
            else
            {
                MessageBox(Art.Gravestone);
                DialogueBox("Unfortunately, the dragon got the better of you and has killed you!");
            }

        }

        private void WizardFound() 
        {
            string[] greetings =
            { 
              $"""
              "Hello {_player.Name}! I must stop you before you delve deeper 
              into the cavities of the Earth."
              """,
              """
              "Travler halt! You're going to need some help on your journey."
              """,
              $"""
              "{_player.Name}! A human face at last!"
              """
            };

            Item potion = RandomPotion();

            MessageBox("You found a wizard!");
            MessageBox(Art.Wizard);
            DialogueBox(greetings[_random.Next(0, greetings.Length)]);
            DialogueBox($"\"Here is a {potion}. It should help you\"");

            string promptString = """
                                  Use the potion now?
                                  Yes/No (y/n)
                                  """;

            char option = PromptBox(promptString, [ 'y', 'n' ]);
            if (option == 'y')
            {
                _player.UseConsumeable(potion);
                if (potion == Item.HealPotion)
                    MessageBox($"Your health is now at {_player.Health}/{Player.MaxHealth}");
                else
                    MessageBox($"Your strength is now at {_player.Strength}/{Player.MaxStrength}");
            }
            else
            {
                _player.GiveItem(potion, 1);
                MessageBox("The potion is now in your inventory");
            }
                
        }

        private Item? ItemPickerMenu(Item start = Item.RustyAxe, Item end = Item.Tape, string title = "Inventory")
        {
            var items =
               _player.Inventory.Where((kv) => kv.Key >= start && kv.Key <= end && kv.Value > 0)
                                .Select((kv, index) => new KeyValuePair<char, KeyValuePair<Item, int>>((char)('a' + index), kv))
                                .ToDictionary();

            if (items.Count == 0)
            {
                MessageBox("You have none");
                return null;
            }

            var promptOptions = items.Select((kv) => kv.Key).ToArray();
            var promptStringRaw = items.Select((kv) => $"{kv.Key}) {kv.Value.Value}x {kv.Value.Key}");
            var promptString = $"{title}\n\n{string.Join('\n', promptStringRaw)}";

            char option = PromptBox(promptString, promptOptions);

            return items[option].Key;
        }

        private void MessageBox(string message) 
        {
            int messageBoxWidth = message.Split('\n').Max(line => line.Length);

            string messageBoxBar = new('═', messageBoxWidth);
            string messageBoxTop = $"╔{messageBoxBar}╗";
            string messageBoxBottom = $"╚{messageBoxBar}╝";

            Console.WriteLine(messageBoxTop);
            using (StringReader stringReader = new(message))
            {
                string? line;
                while ((line = stringReader.ReadLine()) != null)
                {
                    string linePadding = new(' ', messageBoxWidth - line.Length);
                    string lineFormatted = $"║{line}{linePadding}║";

                    Console.WriteLine(lineFormatted);
                }
            }
            Console.WriteLine(messageBoxBottom);
        }

        private void DialogueBox(string dialogue)
        {
            MessageBox(dialogue);
            Console.ReadKey(true);
        }

        private char PromptBox(string prompt, char[] validOptions)
        {
            MessageBox($"{prompt}\n\n> ");

            Console.CursorTop -= 2;
            Console.CursorLeft += 3;

            while (true)
            {
                char chosenOption = Console.ReadKey(true).KeyChar;
                if (validOptions.Contains(chosenOption))
                {
                    Console.Write(chosenOption);
                    Console.CursorTop += 2;
                    Console.CursorLeft = 0;
                    return chosenOption;
                }
            }
        }

        private Item RandomConsumableMisc() => (Item)_random.Next((int)Item.Water, (int)Item.BunchOfNothing + 1);
        private Item RandomWeapon() => (Item)_random.Next((int)Item.IronSword, (int)Item.RubyBroadSword + 1);
        private Item RandomPotion() => (Item)_random.Next((int)Item.HealPotion, (int)Item.StrengthPotion + 1);
    }

    internal enum GameEvent
    {
        Loot,
        Wizard,
        Dragon
    }
}
