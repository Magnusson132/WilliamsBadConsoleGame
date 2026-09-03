using System.Diagnostics;
using WilliamsBadConsoleGame;

int minimumTime = 2;
int maximumTime = 6;
float reactionTime = 0.5f;

PlayerScore playerScore = new PlayerScore();

while (true)
{
    Console.WriteLine("Hello! Welcome to the Stopwatch Game!");
    Console.WriteLine("Press 1 to Play");
    Console.WriteLine("Press 2 to Show Scoreboard");
    Console.WriteLine("Press 3 to Exit");

    ConsoleKey pressedKey = Console.ReadKey(true).Key;

    switch (pressedKey)
    {
        case ConsoleKey.D1:
            Console.WriteLine("\nWait...");
            await Task.Delay(GenerateRandomDuration() * 1000);
            Console.WriteLine("\nGo!");

            Stopwatch stopwatch = Stopwatch.StartNew();

            if (Console.ReadKey(true).Key == ConsoleKey.Spacebar)
            {
                stopwatch.Stop();
                Console.WriteLine($"Great! Reaction Time: {stopwatch.ElapsedMilliseconds} ms");
                playerScore.Score = (int)stopwatch.ElapsedMilliseconds;
            }

            Console.WriteLine("Please enter your name:");
            playerScore.Name = Console.ReadLine();

            ScoreBoardAPI scoreBoardApi = new ScoreBoardAPI();
            await scoreBoardApi.PostScore(playerScore);
            await scoreBoardApi.GetScore();
            break;

        case ConsoleKey.D2:
            ScoreBoardAPI scoreboardApi = new ScoreBoardAPI();
            Console.WriteLine("\nHere is the current scoreboard:");
            await scoreboardApi.GetScore();
            Console.WriteLine("\nGame will now reset to Main Menu");
            Console.WriteLine("\n.");
            Console.WriteLine("\n..");
            Console.WriteLine("\n...");
            break;

        case ConsoleKey.D3:
            Environment.Exit(0);
            break;

        default:
            Console.WriteLine("Invalid selection.");
            break;
    }

    Console.WriteLine(); // blank line before menu repeats, just for readability
}

int GenerateRandomDuration()
{
    Random random = new Random();
    int randomDuration = random.Next(minimumTime, maximumTime);
    Console.WriteLine("The duration of the game is: " + randomDuration);
    return randomDuration;
}