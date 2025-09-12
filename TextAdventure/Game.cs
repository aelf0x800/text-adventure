using System.ComponentModel.Design;
using System.Reflection.Metadata;

namespace TextAdventure
{
    internal class Game
    {
        private Player _player = new("Bob");
        private int _turns = 0;
        private Random _rng = new();

        public void Intro()
        {
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
            while (true)
            {
                char option = PromptBox("? for list of commands", new []{ '?', 'c', 'q', 'i', 'u', 'n' });
                switch (option)
                {
                    case '?':
                        Help();
                        continue;
                    case 'c':
                        Console.Clear();
                        continue;
                    case 'q':
                        Environment.Exit(0);
                        continue;
                    case 'i':
                        ShowInventory();
                        continue;
                    case 'u':
                        UseConsumablePotion();
                        continue;
                    case 'n':
                        break;
                }

                WizardFound();
            }

            //WizardFound();
            //LootFound();
            MonsterFound();
        }

        private void Help()
        {
            MessageBox("""
                       Help
                       
                       ? - Shows this message.
                       c - Clear console.
                       q - Quit the game.
                       i - Show invetory.
                       u - Use consumable/potion.
                       n - Start the next event.
                       """);
        }

        private void ShowInventory()
        {
            string inventoryString = "Inventory\n\n";
            foreach (var keyVal in _player.Inventory)
                inventoryString += $"{keyVal.Value}x {keyVal.Key}\n";

            MessageBox(inventoryString);
        }

        private void UseConsumablePotion()
        {
            /*Dictionary<char, Item> optionToItem = new();
            for (int i = (int)Item.HealPotion; i < (int)Item.Carrot; i++)
            {
                Item consumablePotion = (Item)i;
                char letter =
                if (_player.Inventory.ContainsKey(consumablePotion) && _player.Inventory[consumablePotion] != 0)
                    
            }*/

            /*string consumablesPotionsString = "Consumables/Potions in your inventory\n\n";
            for (int i = (int)Item.HealPotion; i < (int)Item.Carrot; i++)
            {
                Item consumablePotion = (Item)i;
                char letter = 
                if (_player.Inventory.ContainsKey(consumablePotion) && _player.Inventory[consumablePotion] != 0)
                    consumablesPotionsString += $"){_player.Inventory[consumablePotion]}x {consumablePotion}\n";
            }*/

        }

        private void LootFound() 
        {
            Tuple<Item, int> itemA = new(RandomConsumableMisc(), _rng.Next(1, 2));
            Tuple<Item, int> itemB = new(RandomConsumableMisc(), _rng.Next(1, 2));

            string promptString = $"""  
                                  You found some things...
                                  
                                  a) {itemA.Item2}x {itemA.Item1}
                                  b) {itemB.Item2}x {itemB.Item1}
                                  
                                  But you can only take one of them.
                                  So which one will it be?
                                  """;

            char itemChoice = PromptBox(promptString, new []{ 'a', 'b' });
            if (itemChoice == 'a')
                _player.GiveItem(itemA.Item1, itemA.Item2);
            else
                _player.GiveItem(itemB.Item1, itemB.Item2);
        }

        private void MonsterFound() 
        {
            Monster monster = new();
        }

        private void WizardFound() 
        {
            string[] greetings =
            { $"""
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

            MessageBox(Art.Wizard);
            DialogueBox(greetings[_rng.Next(0, greetings.Length)]);
            DialogueBox($"\"Here is a {potion}. It should help you\"");

            string promptString = """
                                  Use the potion now?
                                  Yes/No (y/n)
                                  """;

            char option = PromptBox(promptString, new []{ 'y', 'n' });
            if (option == 'y')
            {
                _player.UsePotion(potion);
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
            Console.ReadKey();
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

        private Item RandomConsumableMisc() => (Item)_rng.Next((int)Item.Water, (int)Item.Count);
        private Item RandomWeapon() => (Item)_rng.Next((int)Item.IronSword, (int)Item.Arrow);
        private Item RandomPotion() => (Item)_rng.Next((int)Item.HealPotion, (int)Item.StrengthPotion);

        /*private int DigitToIndex(char digit) => (int)digit - (int)'0';
        private int LetterToIndex(char letter) => (int)Char.ToLower(letter) - (int)'a';*/
    }
}
