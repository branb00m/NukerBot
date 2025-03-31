using System.Text;
using System.Text.Json;
using DSharpPlus.Entities;

namespace NukerBot.src.Core.Delegating;

internal static class Endpoints
{
    // Discord's main API url
    internal static readonly string API = "https://discord.com/api/";
    internal static readonly string Version = "10";

    // Discord's endpoints
    // These are minimal for now. Again, these bots are only for delegating messages, channels and members

    internal static readonly string Channels = "channels";
    internal static readonly string Emojis = "emojis";
    internal static readonly string Guilds = "guilds";
    internal static readonly string Me = "@me";
    internal static readonly string Members = "members";
    internal static readonly string Messages = "messages";
    internal static readonly string Roles = "roles";
}

internal sealed class InstanceHTTPClient
{
    private readonly HttpClient client = new();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly string _token;

    internal InstanceHTTPClient(string token)
    {
        _token = token;

        client.DefaultRequestHeaders.Add("Authorization", $"User {_token}");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:97.0) Gecko/20100101 Firefox/97.0");
    }

    private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string endpoint, object? payload = null)
    {
        HttpRequestMessage request = new(method, Endpoints.API + '/' + endpoint);

        if (payload != null)
        {
            string json = JsonSerializer.Serialize(payload);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        HttpResponseMessage response = await client.SendAsync(request);

        return response;
    }

    private static StringContent ToJson(object obj) => new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

    private async Task<T?> Deserialize<T>(HttpResponseMessage message)
    {
        if (!message.IsSuccessStatusCode)
        {
            return default!;
        }

        var json = await message.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, _jsonOptions);
    }

    public async Task<DiscordGuild?> GetGuildAsync(ulong ID)
    {
        var response = await SendRequestAsync(HttpMethod.Get, Endpoints.Guilds + '/' + ID);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await Deserialize<DiscordGuild>(response);
    }

    public async Task<DiscordChannel?> GetChannelAsync(ulong ID)
    {
        var response = await SendRequestAsync(HttpMethod.Get, Endpoints.Guilds + '/' + ID);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await Deserialize<DiscordChannel>(response);
    }

    public async Task<bool> SendMessageAsync(ulong ID, string content)
    {
        var payload = new { content };
        var response = await SendRequestAsync(HttpMethod.Post, Endpoints.Channels + '/' + ID, ToJson(payload));

        return response.IsSuccessStatusCode;
    }
}
