using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;

namespace NukerBot.src.Extensions;

public static partial class InteractivityExtensions {
    public static readonly Dictionary<string, DiscordEmoji> NavigationEmojis = new() {
        {
            "Start", DiscordEmoji.FromUnicode("⏪")
        },
        {
            "Backwards", DiscordEmoji.FromUnicode("⬅️")
        },
        {
            "Stop", DiscordEmoji.FromUnicode("⏹")
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

    private static bool ReactionPredicate(CommandContext context, MessageReactionAddEventArgs args)
    {
        var user = args.User;
        var emoji = args.Emoji;

        return context.User == user && NavigationEmojis.ContainsValue(emoji) && context.Channel == args.Channel;
    }

    /// <summary>
    /// This basically is a lazy way to automate `DiscordEmbed` pages
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <returns></returns>
    public static async Task<List<DiscordEmbed>> ToDiscordPages<TObject>(
        this InteractivityExtension _,

        IEnumerable<TObject> objects,

        Func<TObject, string> titleSelector, Func<TObject, string>? descriptionSelector = null,
        Func<TObject, DiscordColor>? colorSelector = null, Func<TObject, (string text, string iconURL)>? footerSelector = null,
        Func<TObject, DateTimeOffset>? timestampSelector = null, Func<TObject, Task<string>>? thumbnailSelector = null,

        params Func<TObject, (string name, string value)>[] selectors)
    {
        var objs = objects.Select(async TObject =>
        {
            var embed = new DiscordEmbedBuilder() {
                Title = titleSelector(TObject)
            };

            if (descriptionSelector != null)
                embed.WithDescription(descriptionSelector(TObject));

            if (colorSelector != null)
                embed.WithColor(colorSelector(TObject));

            if (footerSelector != null) {
                var (text, iconUrl) = footerSelector(TObject);
                embed.WithFooter(text, iconUrl);
            }

            if (timestampSelector != null)
                embed.WithTimestamp(timestampSelector(TObject));

            if (thumbnailSelector != null)
                embed.WithThumbnail(await thumbnailSelector(TObject));

            foreach (var selector in selectors)
            {
                var (name, value) = selector(TObject);

                embed.AddField(name, value);
            }

            return embed.Build();
        });

        return [..await Task.WhenAll(objs)];
    }
}