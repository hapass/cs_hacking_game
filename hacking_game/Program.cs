using System;
using System.Threading.Tasks;

namespace hacking_game
{
    class MainClass
    {
        public static bool isEnd;
        public static bool hasWon;
        public static Random numberRandom;

        public static void Main()
        {
            Sound.Load();
            StartGame();
            StartLoop();
            Sound.Dispose();
        }

        public static async void StartGame()
        {
            numberRandom = new Random();

            await Print("Hi.");
            await Task.Delay(120);
            await Print("Welcome to my game.");
            await Task.Delay(1000);
            await Print("The rules are simple.");
            await Task.Delay(300);
            await Print("I give you the list of numbers.");
            await Task.Delay(600);
            await Print("You try to guess the next one in a list.");
            await Task.Delay(800);
            await Print("Here we go...");

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(900);
                Console.WriteLine(numberRandom.Next());
            }
            await Task.Delay(300);
            await Print("Now... Your turn:");
        }

        public static async Task Print(string message)
        {
            var random = new Random();
            var chars = message.ToCharArray();
            foreach (var ch in chars)
            {
                var timeToFindCharacter = random.Next(10, 100);
                await Task.Delay(timeToFindCharacter);
                Sound.Play();
                Console.Write(ch);
            }

            Console.Write(Environment.NewLine);
        }

        public static void StartLoop()
        {
            while (true)
            {
                ReadInput();

                if (isEnd)
                {
                    Console.WriteLine(hasWon ? "You win life." : "You loose.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    break;
                }
            }
        }

        public static void ReadInput()
        {
            var inputString = Console.ReadLine();
            var isValidInput = int.TryParse(inputString, out var inputInteger);

            if (isValidInput)
            {
                OnInput(inputInteger);
            }
            else
            {
                Console.WriteLine("Please enter an integer.");
            }
        }

        public static void OnInput(int number)
        {
            hasWon = number == numberRandom.Next();
            isEnd = true;
        }
    }
}
