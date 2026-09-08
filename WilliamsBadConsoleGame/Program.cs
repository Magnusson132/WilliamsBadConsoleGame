using System.Diagnostics;
using WilliamsBadConsoleGame;

int minimumTime = 2;
int maximumTime = 6;
float reactionTime = 0.5f;

PlayerScore playerScore = new PlayerScore();

while (true)
{
    Console.WriteLine("\nHello! Welcome to the Stopwatch Game!\n");
    Console.WriteLine("Press 1 to Play");
    Console.WriteLine("Press 2 to Show Scoreboard");
    Console.WriteLine("Press 3 to Show Personal Best\n");
    
    Console.WriteLine("Press 5 to Exit");

    ConsoleKey pressedKey = Console.ReadKey(true).Key;

    switch (pressedKey)
    {
        case ConsoleKey.D1:
            Console.WriteLine("\nAs soon as you see 'Go!' Press Space");
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

            if (playerScore.Score <= 0)
            {
                Console.WriteLine("Invalid score deteced - submission cancelled.");
                break;
            }

            playerScore.Name = GetValidName();

            ScoreBoardAPI scoreBoardApi = new ScoreBoardAPI();
            Console.WriteLine("\nSubmitting...");
            await scoreBoardApi.PostScore(playerScore);
            Console.WriteLine("\nScore submitted!");
            await scoreBoardApi.GetScore();
            break;

        case ConsoleKey.D2:
            ScoreBoardAPI scoreboardApi = new ScoreBoardAPI();
            Console.WriteLine("\nHere is the current scoreboard:");
            await scoreboardApi.GetScore();
            Console.WriteLine("\nGame will now reset to Main Menu");
            break;

        case ConsoleKey.D3:
            string playerNameToCheck = GetValidName();
            ScoreBoardAPI scoreBoardApiPersonal = new ScoreBoardAPI();
            await scoreBoardApiPersonal.GetPersonalBest(playerNameToCheck ?? "");
            break;
        
        case ConsoleKey.D5:
            Environment.Exit(0);
            break;

        default:
            Console.WriteLine("Invalid selection.");
            break;
    }

    Console.WriteLine();
}

int GenerateRandomDuration()
{
    Random random = new Random();
    int randomDuration = random.Next(minimumTime, maximumTime);
    Console.WriteLine("The duration of the game is: " + randomDuration);
    return randomDuration;
}

string GetValidName()
{
    string? name;
    do
    {
        Console.WriteLine("Please enter your name:");
        name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name cannot be blank. Please try again.");
        }
    } while (string.IsNullOrWhiteSpace(name));

    return name;
}