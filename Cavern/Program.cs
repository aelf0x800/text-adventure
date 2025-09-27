namespace Cavern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string name = "";
            while (name == "")
            {
                Console.Write("What is your name? ");
                name = Console.ReadLine();
            }

            Game game = new Game(name);
            game.Intro();
            game.Play();
        }
    }
}
