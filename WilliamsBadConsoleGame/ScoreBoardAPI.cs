using System.Net.Http.Json;

namespace WilliamsBadConsoleGame;

public class ScoreBoardAPI
{
    string scoreboardUrl = "https://scoreboard-csharp-william-default-rtdb.europe-west1.firebasedatabase.app/scores.json";

    public async Task PostScore(PlayerScore playerScore){
        HttpClient client = new HttpClient();
        try {
            // POST while expecting a response
            HttpResponseMessage postResponse = await client.PostAsJsonAsync(scoreboardUrl, playerScore);
	
            postResponse.EnsureSuccessStatusCode();
            // Converts response to readable String
            string result = await postResponse.Content.ReadAsStringAsync();
	
            // Show the converted response
            // Console.WriteLine(result);
        }

        catch (HttpRequestException e){
            Console.WriteLine(e.Message);
        }
    }
	
    public async Task GetScore(){
        try{
            HttpClient client = new HttpClient();

            Dictionary<string, ScoreEntry>? scores =
                await client.GetFromJsonAsync<Dictionary<string, ScoreEntry>>(scoreboardUrl);


            foreach (ScoreEntry score in scores.Values)
            {
                Console.WriteLine($"{score.Name}: {score.Score}");
            }
        }
        catch (HttpRequestException e){
            Console.WriteLine(e.Message);
        }
    }
}