using System.Net.Http.Json;

namespace WilliamsBadConsoleGame;

public class ScoreBoardAPI
{
    string scoreboardUrl = "https://scoreboard-csharp-william-default-rtdb.europe-west1.firebasedatabase.app/scores.json";

    public async Task PostScore(PlayerScore playerScore)
    {
        HttpClient client = new HttpClient();
        try
        {
            HttpResponseMessage postResponse = await client.PostAsJsonAsync(scoreboardUrl, playerScore);
            postResponse.EnsureSuccessStatusCode();
            string result = await postResponse.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task GetScore()
    {
        try
        {
            HttpClient client = new HttpClient();

            Dictionary<string, ScoreEntry>? scores =
                await client.GetFromJsonAsync<Dictionary<string, ScoreEntry>>(scoreboardUrl);

            if (scores == null || scores.Count == 0)
            {
                Console.WriteLine("No scores found on the scoreboard.");
                return;
            }

            var sortedScores = scores.Values
                .OrderBy(s => s.Score)
                .Take(10);

            Console.WriteLine("=== LEADERBOARD ===\n");

            int rank = 1;
            foreach (ScoreEntry score in sortedScores)
            {
                Console.WriteLine($"{rank}. {score.Name,-12} {score.Score}");
                rank++;
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("Network error: " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong: " + e.Message);
        }
    }
    public async Task GetPersonalBest(string playerName)
    {
        try
        {
            HttpClient client = new HttpClient();

            Dictionary<string, ScoreEntry>? scores =
                await client.GetFromJsonAsync<Dictionary<string, ScoreEntry>>(scoreboardUrl);

            if (scores == null || scores.Count == 0)
            {
                Console.WriteLine("No scores found on the scoreboard.");
                return;
            }

            var playerScores = scores.Values
                .Where(s => s.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase))
                .OrderBy(s => s.Score);

            if (!playerScores.Any())
            {
                Console.WriteLine($"No scores found for '{playerName}'.");
                return;
            }

            ScoreEntry best = playerScores.First();
            Console.WriteLine($"{best.Name}'s personal best: {best.Score} ms");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("Network error: " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong: " + e.Message);
        }
    }
}