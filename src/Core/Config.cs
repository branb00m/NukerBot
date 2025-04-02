using DSharpPlus;
using DSharpPlus.Entities;
using Newtonsoft.Json;

namespace NukerBot.src.Core;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class BackupRole
{
    [JsonProperty("name")]
    public string Name { get; internal set; }

    [JsonProperty("color")]
    public string Color { get; internal set; }
}

public class Channel
{
    [JsonProperty("name")]
    public string Name { get; internal set; }

    [JsonProperty("message")]
    public string Message { get; internal set; }
}

public class Client
{
    [JsonProperty("options")]
    public ConfigOptions Options { get; internal set; }
}

public class Filtered
{
    [JsonProperty("members")]
    public List<ulong> Members { get; internal set; }

    [JsonProperty("guilds")]
    public List<ulong> Guilds { get; internal set; }
}

public class Guild
{
    [JsonProperty("name")]
    public string Name { get; internal set; }

    [JsonProperty("reason")]
    public string Reason { get; internal set; }
}

public class Icons
{
    [JsonProperty("guild")]
    public string Guild { get; internal set; }

    [JsonProperty("avatar")]
    public string Avatar { get; internal set; }

    [JsonProperty("emoji")]
    public string Emoji { get; internal set; }

    [JsonProperty("sticker")]
    public string Sticker { get; internal set; }
}

public class Nuking
{
    [JsonProperty("options")]
    public NukingOptions Options { get; internal set; }
}

public class NukingOptions
{
    [JsonProperty("guild")]
    public Guild Guild { get; internal set; }

    [JsonProperty("channel")]
    public Channel Channel { get; internal set; }

    [JsonProperty("icons")]
    public Icons Icons { get; internal set; }

    [JsonProperty("role")]
    public Role Role { get; internal set; }

    [JsonProperty("backup_role")]
    public BackupRole BackupRole { get; internal set; }

    [JsonProperty("filtered")]
    public Filtered Filtered { get; internal set; }

    [JsonProperty("delegating")]
    public Delegation Delegation { get; internal set; }

    [JsonProperty("protection_bots")]
    public List<ProtectionBot> ProtectionBots { get; internal set; }

    [JsonProperty("impersonation_bots")]
    public List<ImpersonationBot> ImpersonationBots { get; internal set; }

    [JsonProperty("accepted")]
    public bool Accepted { get; internal set; }
}

public class Delegation
{
    [JsonProperty("tokens")]
    public List<string> Tokens { get; internal set; }
}

public class ImpersonationBot
{
    [JsonProperty("name")]
    public string Name { get; internal set; }

    [JsonProperty("id")]
    public ulong ID { get; internal set; }

    public static explicit operator DiscordUser(ImpersonationBot bot)
    {
        throw new InvalidOperationException("A DiscordClient instance is required for this explicit conversion");
    }

    public static async Task<DiscordUser> ToDiscordUserAsync(ImpersonationBot bot, DiscordClient client)
    {
        Console.WriteLine(bot.ID);
        return await client.GetUserAsync(bot.ID, true);
    }

    public static explicit operator DiscordMember(ImpersonationBot bot)
    {
        throw new InvalidOperationException("A DiscordGuild instance is required for this explicit conversion");
    }

    public static async Task<DiscordUser> ToDiscordMemberAsync(ImpersonationBot bot, DiscordGuild guild)
    {
        return await guild.GetMemberAsync(bot.ID, true);
    }
}

public class ConfigOptions
{
    [JsonProperty("owner_ids")]
    public List<ulong> OwnerIDs { get; internal set; }

    [JsonProperty("prefixes")]
    public List<string> Prefixes { get; internal set; }

    [JsonProperty("token")]
    public string Token { get; internal set; }
}

public class ProtectionBot
{
    [JsonProperty("aliases")]
    public List<string> Aliases { get; internal set; }

    [JsonProperty("name")]
    public string Name { get; internal set; }

    [JsonProperty("id")]
    public ulong ID { get; internal set; }

    public static explicit operator DiscordUser(ProtectionBot bot)
    {
        throw new InvalidOperationException("A DiscordClient instance is required for this explicit conversion");
    }

    public static async Task<DiscordUser> ToDiscordUserAsync(ProtectionBot bot, DiscordClient client)
    {
        return await client.GetUserAsync(bot.ID);
    }

    public static explicit operator DiscordMember(ProtectionBot bot)
    {
        throw new InvalidOperationException("A DiscordGuild instance is required for this explicit conversion");
    }

    public static async Task<DiscordUser> ToDiscordMemberAsync(ProtectionBot bot, DiscordGuild guild)
    {
        return await guild.GetMemberAsync(bot.ID);
    }
}

public class Default
{
    [JsonProperty("name")]
    public string Name { get; internal set; }

    [JsonProperty("avatar")]
    public string Avatar { get; internal set; }
}

public class Role
{
    [JsonProperty("name")]
    public string Name { get; internal set; }

    [JsonProperty("color")]
    public string Color { get; internal set; }
}

public class Config
{
    [JsonProperty("client")]
    public Client Client { get; internal set; }

    [JsonProperty("nuking")]
    public Nuking Nuking { get; internal set; }

    public static Config Deserialize(string @jsonPath)
    {
        if (string.IsNullOrEmpty(jsonPath) || string.IsNullOrWhiteSpace(jsonPath))
        {
            throw new NullReferenceException($"{nameof(jsonPath)} cannot be empty, null or whitespace");
        }

        if (!File.Exists(jsonPath))
        {
            throw new FileNotFoundException(nameof(jsonPath));
        }

        string data = File.ReadAllText(jsonPath);

        var config = JsonConvert.DeserializeObject<Config>(data) ?? throw new NullReferenceException("config is null");

        return config;
    }

    public new static Type GetType() => typeof(Config);
}


#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
