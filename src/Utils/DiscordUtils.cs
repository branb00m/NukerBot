using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using NukerBot.src.Core;

namespace NukerBot.src.Utils;

public static class DiscordUtils
{
    public static async Task<IEnumerable<DiscordMember>> GetDiscordMembersAsync(DiscordGuild guild, Config config)
    {
        var members = await guild.GetAllMembersAsync();

        return
            from member in members

            where guild.CurrentMember.Hierarchy > member.Hierarchy
            where config.Nuking.Options.Filtered.Members.Contains(member.Id)

            select member;
    }

    public static async Task<IEnumerable<DiscordMember>> GetDiscordMembersAsync(CommandContext context, Config config)
    {
        var guild = context.Guild;
        var members = await guild.GetAllMembersAsync();

        return
            from member in members

            where guild.CurrentMember.Hierarchy > member.Hierarchy
            where !config.Nuking.Options.Filtered.Members.Contains(member.Id)

            select member;
    }

    public static IEnumerable<DiscordRole> GetDiscordRolesAsync(CommandContext context, Config config)
    {
        var guild = context.Guild;
        var roles = guild.Roles.Values;

        foreach (var role in roles)
        {
            var myRole = context.Guild.CurrentMember.Roles.First();
            if (myRole.Position > role.Position && role.Name != config.Nuking.Options.BackupRole.Name)
            {
                yield return role;
            }
        }

        yield break;
    }

    public static IEnumerable<DiscordRole> GetDiscordRolesAsync(DiscordGuild guild, Config config)
    {
        var roles = guild.Roles.Values;

        return
            from role in roles
            let myRole = guild.CurrentMember.Roles.First()
            where myRole.Position > role.Position
            select role;
    }

    public static async Task<byte[]> DownloadAvatarAsync(DiscordUser user)
    {
        using HttpClient client = new();
        var byteArray = await client.GetByteArrayAsync(user.AvatarUrl);

        return byteArray;
    }
}
