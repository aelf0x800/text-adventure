using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    internal class Person
    {
        public const string YoungMaleArt = "";
        public const string OldMaleArt = "";
        public const string YoungFemaleArt = "";
        public const string OldFemaleArt = "";
        public const string WizardArt = "                       .\n             /^\\     .\n        /\\   \"V\"\n       /__\\   I      O  o\n      //..\\\\  I     .\n      \\].`[/  I\n      /l\\/j\\  (]    .  O\n     /. ~~ ,\\/I          .\n     \\\\L__j^\\/I       o\n      \\/--v}  I     o   .\n      |    |  I   _________\n      |    |  I c(`       ')o \n      |    l  I   \\.     ,/       \n     _/j  L l\\_!  _//^---^\\\\_     \n  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  ";

        public string Name { get; private set; }
        public int Age { get; private set; }

        public Person()
        {
            string[] maleNames = { "Noah", "Oliver", "Arthur", "Leo", "George", "Luke", "Theodore", "Oscar", "Archie", "Freddie", "Henry", "Arlo", "Alfie " };
            string[] femaleNames = { "Amelia", "Ava", "Ivy", "Freya", "Lily", "Florence", "Mia", "Willow", "Rosie", "Sophia", "Isabella", "Grace", "Daisy" };

            Random rng = new Random();

            Age = rng.Next(10, 90);
            if (rng.Next(0, 1) == 0)
                Name = maleNames[rng.Next(0, maleNames.Length - 1)];
            else
                Name = femaleNames[rng.Next(0, femaleNames.Length - 1)];
        }
    }
}
