using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;

namespace NukerBot.src.Extensions;

public static class InteractivityExtensions {
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

    public static async Task<DiscordMessage> AddNavigationReactionsAsync(this InteractivityExtension interactivity, CommandContext context, List<DiscordEmbed> embeds)
    {
        var embedMessage = await context.Channel.SendMessageAsync(embed: embeds[0]);

        foreach (var emoji in NavigationEmojis.Values)
        {
            await embedMessage.CreateReactionAsync(emoji);
        }

        return embedMessage;
    }

    public static async Task DoNavigationEmojisAsync(
        this InteractivityExtension interactivity,

        CommandContext context,
        DiscordMessage message,

        TimeSpan timeout,

        IReadOnlyList<DiscordEmbed> embeds)
    {
        int currentPage = 0;

        while (true)
        {
            var awaitedReaction = await interactivity.WaitForReactionAsync(x => ReactionPredicate(context, x), timeout);
            var result = awaitedReaction.Result;

            if (awaitedReaction.TimedOut)
            {
                await message.ModifyAsync("Navigation stopped");
                await message.DeleteAllReactionsAsync();

                break;
            }

            if (result.Emoji == NavigationEmojis["Forwards"] && currentPage < embeds.Count - 1)
            {
                currentPage++;

                await message.ModifyAsync(embeds[currentPage]);
            }
            else if (result.Emoji == NavigationEmojis["End"] && currentPage < embeds.Count - 1)
            {
                currentPage = embeds.Count - 1;

                await message.ModifyAsync(embeds[currentPage]);
            }
            else if (result.Emoji == NavigationEmojis["Stop"])
            {
                await message.ModifyAsync("Navigation stopped");
                await message.DeleteAllReactionsAsync();

                break;
            }
            else if (result.Emoji == NavigationEmojis["Start"] && currentPage < embeds.Count - 1)
            {
                currentPage = 0;

                await message.ModifyAsync(embeds[currentPage]);
            }
            else if (result.Emoji == NavigationEmojis["Backwards"] && currentPage > 0)
            {
                currentPage--;

                await message.ModifyAsync(embeds[currentPage]);
            }

            await message.DeleteReactionAsync(result.Emoji, result.User);
        }
    }

    public static bool ReactionPredicate(CommandContext context, MessageReactionAddEventArgs args)
    {
        var user = args.User;
        var emoji = args.Emoji;

        return context.User == user && NavigationEmojis.ContainsValue(emoji) && context.Channel == args.Channel;
    }
}