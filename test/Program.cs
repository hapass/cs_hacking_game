using System;
using System.Threading.Tasks;

namespace test
{
    class MainClass
    {
        public const int GuessingNumber = 3234;
        public static bool isEnd = false;
        public static bool hasWon = false;

        public static void Main(string[] args)
        {
            StartGame();
            StartLoop();
        }

        public static async void StartGame()
        {
            await Task.Delay(200);
            await Print("Hi.");
            await Task.Delay(120);
            await Print("Welcome to my game.");
            await Task.Delay(1000);
            await Print("The rules are simple. You try to guess next number.");
        }

        public static async Task Print(string message)
        {
            var random = new Random();
            var chars = message.ToCharArray();
            foreach(var ch in chars)
            {
                var timeToFindCharacter = random.Next(10, 100);
                await Task.Delay(timeToFindCharacter);
                Console.Write(ch);
            }

            Console.Write(Environment.NewLine);
        }

        public static void StartLoop()
        {
            while (true)
            {
                ReadInput();

                if(isEnd)
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
            hasWon = number == GuessingNumber;
            isEnd = true;
        }
    }
}
