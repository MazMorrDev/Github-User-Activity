# GitHub User Activity CLI - .NET Version

A .NET-based command-line interface tool that fetches and displays recent GitHub user activity using the GitHub API. This project demonstrates API integration, JSON data handling, and CLI application development in .NET.

## Features

- 📥 Fetch recent activity of any GitHub user
- 🖥️ Clean terminal output displaying user events
- 🔍 Support for multiple event types (pushes, issues, stars, etc.)
- ⚠️ Graceful error handling for invalid usernames and API failures
- 🚀 Built with native .NET libraries (no external NuGet packages for API calls)

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) 8.0 or later
- Internet connection to access GitHub API

## Installation

1. Clone the repository:

```bash
git clone https://github.com/yourusername/github-activity.git
cd github-activity
```

2. Build the project:

```bash
dotnet build
```

3. (Optional) Publish as a single executable:

```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
# For Linux: -r linux-x64
# For macOS: -r osx-x64
```

## Usage

Run the application with a GitHub username as an argument:

```bash
dotnet run -- <username>
```

Or if published as an executable:

```bash
./github-activity <username>
```

Example:

```bash
dotnet run -- kamranahmedse
```

### Sample Output

```text
Fetching recent activity for kamranahmedse...

Recent Activity:
• Pushed 3 commits to kamranahmedse/developer-roadmap
• Opened a new issue in kamranahmedse/developer-roadmap
• Starred kamranahmedse/developer-roadmap
• Created repository: kamranahmedse/awesome-project
• Forked octocat/Hello-World
• Released v1.0.0 in kamranahmedse/awesome-project
```

## Project Structure

```text
github-activity/
├── Program.cs                 # Main entry point
├── Models/
│   ├── GitHubEvent.cs        # Event data models
│   └── GitHubEventType.cs    # Event type enumeration
├── Services/
│   ├── GitHubApiService.cs   # API interaction
│   └── ActivityFormatter.cs  # Output formatting
├── github-activity.csproj    # Project file
└── README.md                  # This file
```

## How It Works

The application:

1. Takes a GitHub username as a command-line argument
2. Uses `HttpClient` to make a GET request to the GitHub API endpoint: `https://api.github.com/users/{username}/events`
3. Parses the JSON response using `System.Text.Json`
4. Formats and displays the recent activity in a readable format
5. Handles various error scenarios with try-catch blocks

## Event Types Displayed

The tool formats various GitHub event types:

| Event Type | Display Format |
|------------|----------------|
| PushEvent | `Pushed X commits to {repo}` |
| IssuesEvent | `{Action} issue in {repo}` (opened/closed/reopened) |
| WatchEvent | `Starred {repo}` |
| CreateEvent | `Created {type} in {repo}` (repository/branch/tag) |
| ForkEvent | `Forked {repo}` |
| PullRequestEvent | `{Action} pull request in {repo}` |
| IssueCommentEvent | `Commented on issue in {repo}` |
| DeleteEvent | `Deleted {type} in {repo}` |
| ReleaseEvent | `Released {tag} in {repo}` |

## Error Handling

The application gracefully handles:

- ❌ Invalid or non-existent usernames
- 🌐 Network connection issues
- ⏱️ GitHub API rate limiting (returns 403/429 status codes)
- 📡 API response errors
- 🔍 No recent activity found
- 🚫 Missing username argument

## Building from Source

### Debug Build

```bash
dotnet build
dotnet run -- testuser
```

### Release Build

```bash
dotnet build -c Release
dotnet run -c Release -- testuser
```

### Platform-Specific Executables

**Windows:**

```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

**Linux:**

```bash
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true
```

**macOS:**

```bash
dotnet publish -c Release -r osx-x64 --self-contained true /p:PublishSingleFile=true
```

## Testing

Run the tests (if implemented):

```bash
dotnet test
```

## Contributing

Contributions are welcome! Here are some ways to improve the project:

- Add activity filtering by event type (`--filter push`)
- Implement caching to reduce API calls
- Add support for different output formats (JSON, CSV)
- Include activity statistics (total commits, repos starred)
- Add color-coded output
- Implement unit tests
- Add pagination support for more than 30 events

## Advanced Features Roadmap

- [ ] Filter activities by type (`--filter push`, `--filter issues`)
- [ ] Display activity from specific time ranges (`--since 7d`)
- [ ] Show activity statistics summary
- [ ] Color-coded console output
- [ ] Export to file functionality
- [ ] Multiple user comparison
- [ ] Repository details integration

## Dependencies

This project uses only .NET native libraries:

- `System.Net.Http` - For API requests
- `System.Text.Json` - For JSON parsing
- `System.Threading.Tasks` - For async operations

No external NuGet packages are required for core functionality.

## License

This project is open source and available under the [MIT License](LICENSE).

## Acknowledgments

- GitHub API documentation
- Inspired by [roadmap.sh](https://roadmap.sh/projects/github-user-activity) project idea
- Built with .NET

## Contact

For questions or suggestions, please open an issue in the repository.

---

**Note:** This project is for educational purposes and demonstrates working with APIs, JSON data, and CLI applications in .NET. Feel free to use, modify, and enhance it as needed.
