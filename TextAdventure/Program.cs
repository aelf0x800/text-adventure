using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Cryptography;

namespace TextAdventure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Intro();
            game.Play();

            //Intro();

        }

        /*static void Intro()
        {
            DrawMessageBox("It's 1 am...", true, true);
            DrawMessageBox("Sounds are comming from the village's local gemstone mines.\nSounds unlike any heard before...", true, true);
            DrawMessageBox("The village remains in a deep sleep, undisturbed by the sounds, while you grow curious...", true, true);
            DrawMessageBox("The curiosity takes over, and you begin to prepare your \"investigation\"...", true, true);
            DrawMessageBox("Supplies packed, a torch in one hand, and an old rusty axe in the other,\nyou begin your journey into the mines.", true, true);
        }*/
    }

    
    
    
    
}
