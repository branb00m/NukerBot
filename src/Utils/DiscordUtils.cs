using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace NukerBot.src.Utils;

public static class DiscordUtils
{
    public static readonly Dictionary<string, DiscordEmoji> NavigationEmojis = new() {
        {
            "Start", DiscordEmoji.FromUnicode("⏪")
        },
        {
            "Backwards", DiscordEmoji.FromUnicode("⬅️")
        },
        {
            "Stop", DiscordEmoji.FromUnicode("⏸️")
        },
        {
            "Forwards", DiscordEmoji.FromUnicode("➡️")
        },
        {
            "End", DiscordEmoji.FromUnicode("⏩")
        }
    };

    public static async Task<DiscordMessage> AddNavigationReactions(CommandContext context, List<DiscordEmbed> embeds)
    {
        var embedMessage = await context.Channel.SendMessageAsync(embed: embeds[0]);

        foreach (var emoji in NavigationEmojis.Values)
        {
            await embedMessage.CreateReactionAsync(emoji);
        }

        return embedMessage;
    }

    public static async Task<DiscordUser> GetProtectionBotAsync(ProtectionBot bot, DiscordClient client)
    {
        var protectionBot = await client.GetUserAsync(bot.ID, true);

        return protectionBot;
    }

    public static bool ReactionPredicate(CommandContext context, MessageReactionAddEventArgs args)
    {
        var user = args.User;
        var emoji = args.Emoji;

        return context.User == user && NavigationEmojis.ContainsValue(emoji) && context.Channel == args.Channel;
    }

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
            where config.Nuking.Options.Filtered.Members.Contains(member.Id)

            select member;
    }

    public static IEnumerable<DiscordRole> GetDiscordRolesAsync(CommandContext context, Config config)
    {
        var guild = context.Guild;
        var roles = guild.Roles.Values;

        return
            from role in roles
            let myRole = context.Guild.CurrentMember.Roles.First()
            where myRole.Position > role.Position
            select role;
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
}
