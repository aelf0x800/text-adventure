using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextAdventure
{
    internal class Game
    {
        private Player _player = new Player("Placeholder");
        private int _turns = 0;
        private Random _rng = new Random();

        public void Intro()
        {
            MessageBox("     .       *       .         .         .\n          .       .     *        .       .\n  *               .         .       *       . \n       .       .      *         .       *\n\n     /\\         /\\         /\\         /\\\n    /  \\       /  \\       /  \\       /  \\\n   /____\\     /____\\     /____\\     /____\\\n   |    |     |    |     |    |     |    |\n   | [] |     | [] |     | [] |     | [] |\n   |____|     |____|     |____|     |____|\n");
            DialogueBox("It's 1 am...");

            MessageBox("  __________________________  \n  ||=/  ______________  \\=||  \n  ||/  /       S      \\  \\||  \n  |/  /       [-]      \\  \\|  \n  |  |                  |  |  \n  |  |                  |  |  \n  |  |        (sounds)  |  |  \n  |  |                  |  |  \n  |  | ^     /===\\ ^  ^ |  |  \n  |  |   ^  /=====\\  ^  |  |  \n  |^^| ^   /=======\\    |^^|  \n  ^   ^   /=========\\ ^     ^ \n    ^  ^ /===========\\  ^  ^   ");
            DialogueBox("Sounds are comming from the village's local gemstone mines.\nSounds unlike any heard before...");

            MessageBox("     .       *       .         .         .\n          .       .     *        .       .\n  *               .         .       *       . \n       .       .      *         .       *\n     /\\         /\\         /\\         /\\\n    /  \\ z   z /  \\ z     /  \\ z     /  \\ z\n   /____z     z____Z     /____z     Z____Z   \n   |   z|     |Z  z|     |   Z|     |z  Z|\n   | [] |     | [] |     | [] |     | [] |\n   |____|     |____|     |____|     |____|");
            DialogueBox("The village remains in a deep sleep, undisturbed by the sounds, while you grow curious...");

            MessageBox("  __________________________  \n  ||=/  ______________  \\=||  \n  ||/  /       S      \\  \\||  \n  |/  /       [*]      \\  \\|  \n  |  |                  |  |  \n  |  |        ()        |  |  \n  |  |       /[]\\       |  |  \n  |  |        /\\        |  |  \n  |  | ^           ^  ^ |  |  \n  |  |   ^  /=====\\  ^  |  |  \n  |^^| ^   /=======\\    |^^|  \n  ^   ^   /=========\\ ^     ^ \n    ^  ^ /===========\\  ^  ^   ");
            DialogueBox("The curiosity takes over, and you begin to prepare your \"investigation\"...");
            DialogueBox("Supplies packed, a torch in one hand, and an old rusty axe in the other,\nyou begin your journey into the mines.");
        }

        public void Play()
        {
            LootFound();
            WizardFound();

        }

        private void LootFound() 
        {
            Tuple<Item, int>[] items =
            {
                new(RandomConsumableMisc(), _rng.Next(1, 2)),
                new(RandomConsumableMisc(), _rng.Next(1, 2)),
                new(RandomConsumableMisc(), _rng.Next(1, 2)),
            };

            string itemsString = "";
            for (int i = 0; i < items.Length; i++)
                itemsString += $"{i}. {items[i].Item2}x {items[i].Item1}\n";

            int itemChoice = DigitToIndex(PromptBox($"You found some things...\n\n{itemsString}\n\nBut you can only take one item.\nSo which one will it be?", new char[]{ '0', '1', '2' }));
            if (!_player.Inventory.ContainsKey(items[itemChoice].Item1))
                _player.Inventory.Add(items[itemChoice].Item1, items[itemChoice].Item2);
            else
                _player.Inventory[items[itemChoice].Item1] += items[itemChoice].Item2;

            MessageBox($"You just gained {items[itemChoice].Item2} extra {items[itemChoice].Item1}");
        }

        private void PersonFound() { }

        private void MonsterFound() { }
        private void WizardFound() 
        {
            MessageBox(Person.WizardArt);
        }

        private void MessageBox(string message) 
        {
            List<string> messageLines = new List<string>(message.Split('\n'));

            int messageBoxHeight = messageLines.Count;
            int messageBoxWidth = 0;
            foreach (var line in messageLines)
                if (line.Length > messageBoxWidth)
                    messageBoxWidth = line.Length;

            Console.WriteLine($"╔{new string('═', messageBoxWidth)}╗");
            foreach (var line in messageLines)
                Console.WriteLine($"║{line}{new string(' ', messageBoxWidth - line.Length)}║");
            Console.WriteLine($"╚{new string('═', messageBoxWidth)}╝");
        }

        private void DialogueBox(string dialogue)
        {
            MessageBox($"{dialogue}\n\n[Press any key to continue...]");

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

        private Item RandomConsumableMisc() => (Item)_rng.Next((int)Item.Water, (int)Item.Count);
        private Item RandomWeapon() => (Item)_rng.Next((int)Item.IronSword, (int)Item.Arrow);
        private Item RandomPotion() => (Item)_rng.Next((int)Item.HealPotion, (int)Item.StrengthPotion);

        private int DigitToIndex(char digit) => (int)digit - (int)'0';
        private int LetterToIndex(char letter) => (int)Char.ToLower(letter) - (int)'a';
    }
}
