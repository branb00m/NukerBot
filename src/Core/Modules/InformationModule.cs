using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using NukerBot.src.Extensions;

namespace NukerBot.src.Core.Modules;

public sealed class InformationModule : BaseCommandModule
{
    public required Config Config { get; set; }

    [Command("listprotectionbots")]
    [Description("This command lists enemy bots (Anti-nuke bots)")]
    [Aliases("getprotectionbots", "gpb", "lpb")]
    public async Task GetProtectionBots(CommandContext context)
    {
        var protectionBots = Config.Nuking.Options.ProtectionBots;

        var interactivity = context.Client.GetInteractivity();
        var pages = await interactivity.ToDiscordPages(
            protectionBots,
            titleSelector: x => x.Name,
            colorSelector: x => DiscordColor.Blue,
            footerSelector: x => ($"Anti-Nuke bots that we so, SOOO hate...", context.Client.CurrentUser.AvatarUrl),
            thumbnailSelector: async x =>
            {
                var user = await ProtectionBot.ToDiscordUserAsync(x, context.Client); // Uses explicit conversion
                return user.AvatarUrl;
            },

            selectors: x => ("aliases", string.Join(",\n   ", x.Aliases.Select(x => $"`{x}`")))
        );
        var message = await interactivity.AddNavigationReactionsAsync(context, pages);

        await interactivity.DoNavigationEmojisAsync(context, message, TimeSpan.FromSeconds(30), pages);
    }
}