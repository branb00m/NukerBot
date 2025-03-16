using DSharpPlus.Entities;
using Newtonsoft.Json;

namespace NukerBot.src;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class BackupRole
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("color")]
    public string Color { get; set; }
}

public class Channel
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}

public class Client
{
    [JsonProperty("options")]
    public ConfigOptions Options { get; set; }
}

public class Filtered
{
    [JsonProperty("members")]
    public List<ulong> Members { get; set; }

    [JsonProperty("guilds")]
    public List<ulong> Guilds { get; set; }
}

public class Guild
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("reason")]
    public string Reason { get; set; }
}

public class Icons
{
    [JsonProperty("guild")]
    public string Guild { get; set; }

    [JsonProperty("avatar")]
    public string Avatar { get; set; }

    [JsonProperty("emoji")]
    public string Emoji { get; set; }

    [JsonProperty("sticker")]
    public string Sticker { get; set; }
}

public class Nuking
{
    [JsonProperty("options")]
    public NukingOptions Options { get; set; }
}

public class NukingOptions
{
    [JsonProperty("guild")]
    public Guild Guild { get; set; }

    [JsonProperty("channel")]
    public Channel Channel { get; set; }

    [JsonProperty("icons")]
    public Icons Icons { get; set; }

    [JsonProperty("role")]
    public Role Role { get; set; }

    [JsonProperty("backup_role")]
    public BackupRole BackupRole { get; set; }

    [JsonProperty("filtered")]
    public Filtered Filtered { get; set; }

    [JsonProperty("protection_bots")]
    public List<ProtectionBot> ProtectionBots { get; set; }
}

public class ConfigOptions
{
    [JsonProperty("owner_ids")]
    public List<ulong> OwnerIDs { get; set; }

    [JsonProperty("prefixes")]
    public List<string> Prefixes { get; set; }

    [JsonProperty("token")]
    public string Token { get; set; }

    [JsonProperty("logging")]
    public bool Logging { get; set; }
}

public class ProtectionBot
{
    [JsonProperty("aliases")]
    public List<string> Aliases { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("id")]
    public ulong ID { get; set; }

    public static explicit operator DiscordMember(ProtectionBot bot) {
        throw new InvalidOperationException("A DiscordGuild instance is required for this explicit conversion");
    }

    public static async Task<DiscordMember> ToDiscordMemberAsync(ProtectionBot bot, DiscordGuild guild) {
        return await guild.GetMemberAsync(bot.ID);
    }
}


public class Role
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("color")]
    public string Color { get; set; }
}

public class Config
{
    [JsonProperty("client")]
    public Client Client { get; set; }

    [JsonProperty("nuking")]
    public Nuking Nuking { get; set; }

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
}


#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
