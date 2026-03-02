using System.Text.Json;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: github-activity <username>");
            Console.WriteLine("Example: github-activity kamranahmedse");
            return;
        }

        string username = args[0];
        await GetGitHubActivityAsync(username);
    }

    static string FormatPushEvent(JsonElement eventItem, string repo)
    {
        int commitCount = 0;
        if (eventItem.TryGetProperty("payload", out JsonElement payload))
        {
            if (payload.TryGetProperty("commits", out JsonElement commits))
            {
                commitCount = commits.GetArrayLength();
            }
        }

        return commitCount > 0
            ? $"- Pushed {commitCount} commits to {repo}"
            : $"- Pushed to {repo}";
    }

    static string FormatIssueEvent(JsonElement eventItem, string repo)
    {
        string action = "unknown";
        if (eventItem.TryGetProperty("payload", out JsonElement payload))
        {
            if (payload.TryGetProperty("action", out JsonElement actionElement))
            {
                action = actionElement.GetString() ?? "unknown";
            }
        }

        return $"- {action} issue in {repo}";
    }

    static string FormatPullRequestEvent(JsonElement eventItem, string repo)
    {
        string action = "unknown";
        if (eventItem.TryGetProperty("payload", out JsonElement payload))
        {
            if (payload.TryGetProperty("action", out JsonElement actionElement))
            {
                action = actionElement.GetString() ?? "unknown";
            }
        }

        return $"- {action} pull request in {repo}";
    }

    static async Task GetGitHubActivityAsync(string username)
    {
        using HttpClient client = new();

        try
        {
            client.DefaultRequestHeaders.Add("User-Agent", "GitHub-Activity-CLI");

            string url = $"https://api.github.com/users/{username}/events";
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Requisito 3: Formato de salida requerido
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                JsonElement root = doc.RootElement;

                Console.WriteLine($"\nRecent activity for {username}:");
                Console.WriteLine("----------------------------------------");

                if (root.GetArrayLength() == 0)
                {
                    Console.WriteLine("No recent activity found.");
                }

                foreach (JsonElement eventItem in root.EnumerateArray())
                {
                    string? type = eventItem.GetProperty("type").GetString();
                    string? repo = eventItem.GetProperty("repo").GetProperty("name").GetString();

                    if (type == null || repo == null) continue;

                    // Formatear según el tipo de evento
                    string output = type switch
                    {
                        "PushEvent" => FormatPushEvent(eventItem, repo),
                        "IssuesEvent" => FormatIssueEvent(eventItem, repo),
                        "WatchEvent" => $"- Starred {repo}",
                        "ForkEvent" => $"- Forked {repo}",
                        "CreateEvent" => $"- Created repository/branch in {repo}",
                        "PullRequestEvent" => FormatPullRequestEvent(eventItem, repo),
                        "DeleteEvent" => $"- Deleted branch in {repo}",
                        "ReleaseEvent" => $"- Released in {repo}",
                        _ => $"- {type} in {repo}"
                    };

                    Console.WriteLine(output);
                }
            }
            else
            {
                // Requisito 4: Manejo de errores específico
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        Console.WriteLine($"Error: User '{username}' not found");
                        break;
                    case System.Net.HttpStatusCode.Forbidden:
                        Console.WriteLine("Error: API rate limit exceeded. Try again later.");
                        break;
                    default:
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        break;
                }
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Connection error: {ex.Message}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error parsing response: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}