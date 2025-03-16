using System.Text;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using NukerBot.src.Utils;

namespace NukerBot.src.Modules;

public sealed class Basic : BaseCommandModule
{
    public required Config Config { get; set; }

    [Command("listprotectionbots")]
    [Description("This command lists enemy bots (Anti-nuke bots)")]
    [Aliases("getprotectionbots", "gpb", "lpb")]
    public async Task GetProtectionBots(CommandContext context)
    {
        List<DiscordEmbed> embeds = [];

        var protectionBots = Config.Nuking.Options.ProtectionBots;

        var currentPage = 0;

        if(protectionBots.Count <= 0) {
            await context.RespondAsync("Whatever you've done, you've fucked up the bot. I told your ass not TO TOUCH THAT CHUNK");
        } 

        for (int i = 0; i < protectionBots.Count; i++)
        {
            var protectionBot = protectionBots[i];
            var fetched = await DiscordUtils.GetProtectionBotAsync(protectionBot, context.Client);

            var embed = new DiscordEmbedBuilder()
                .WithColor(0xff0000)
                .WithTitle($"{protectionBot.Name}, bot {i + 1}/{protectionBots.Count}")
                .WithFooter($"Anti-Nuke bots that we so, SOOO hate...", context.Client.CurrentUser.AvatarUrl)
                .WithThumbnail(fetched.AvatarUrl);

            embed.AddField("aliases", string.Join(",\n   ", protectionBot.Aliases.Select(x => $"`{x}`")));

            embeds.Add(embed);
        }

        var embedMessage = await DiscordUtils.AddNavigationReactions(context, embeds);

        var interactivity = context.Client.GetInteractivity();

        while (true)
        {
            var awaitedReaction = await interactivity.WaitForReactionAsync(x => DiscordUtils.ReactionPredicate(context, x));
            var result = awaitedReaction.Result;

            if (awaitedReaction.TimedOut)
            {
                await embedMessage.ModifyAsync("Navigation stopped");
                await embedMessage.DeleteAllReactionsAsync();

                break;
            }

            if (result.Emoji == DiscordUtils.NavigationEmojis["Forwards"] && currentPage < embeds.Count - 1)
            {
                currentPage++;

                await embedMessage.ModifyAsync(embeds[currentPage]);
            }
            else if (result.Emoji == DiscordUtils.NavigationEmojis["End"] && currentPage < embeds.Count - 1)
            {
                currentPage = embeds.Count - 1;

                await embedMessage.ModifyAsync(embeds[currentPage]);
            }
            else if (result.Emoji == DiscordUtils.NavigationEmojis["Stop"])
            {
                await embedMessage.ModifyAsync("Navigation stopped");
                await embedMessage.DeleteAllReactionsAsync();

                break;
            }
            else if (result.Emoji == DiscordUtils.NavigationEmojis["Start"] && currentPage < embeds.Count - 1)
            {
                currentPage = 0;

                await embedMessage.ModifyAsync(embeds[currentPage]);
            }
            else if (result.Emoji == DiscordUtils.NavigationEmojis["Backwards"] && currentPage > 0)
            {
                currentPage--;

                await embedMessage.ModifyAsync(embeds[currentPage]);
            }

            await embedMessage.DeleteReactionAsync(result.Emoji, result.User);
        }
    }

    [Command("viewconfig")]
    [Aliases("vconfig")]
    [Description("Allows you to see your config properties")]
    public async Task ViewConfigAsync(CommandContext context) {
        var embed = new DiscordEmbedBuilder()
            .WithTitle("Config properties")
            .WithColor(0xff0000)
            .WithDescription();
    }
}