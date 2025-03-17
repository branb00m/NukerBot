using System.Text;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using NukerBot.src.Extensions;
using NukerBot.src.Utils;

namespace NukerBot.src.Modules;

public sealed class BasicModule : BaseCommandModule
{
    public required Config Config { get; set; }

    [Command("listprotectionbots")]
    [Description("This command lists enemy bots (Anti-nuke bots)")]
    [Aliases("getprotectionbots", "gpb", "lpb")]
    public async Task GetProtectionBots(CommandContext context)
    {
        List<DiscordEmbed> embeds = [];

        var protectionBots = Config.Nuking.Options.ProtectionBots;

        if (protectionBots.Count <= 0)
        {
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

        var interactivity = context.Client.GetInteractivity();
        var message = await interactivity.AddNavigationReactionsAsync(context, embeds);

        await interactivity.DoNavigationEmojisAsync(context, message, TimeSpan.FromSeconds(30), embeds);
    }

    [Command("viewconfig")]
    [Aliases("vconfig")]
    [Description("Allows you to see your config properties")]
    public async Task ViewConfigAsync(CommandContext context)
    {
        var embed = new DiscordEmbedBuilder()
            .WithTitle("Config properties")
            .WithColor(0xff0000);

        await context.RespondAsync(Config.Nuking.Options.Channel.Name.MaskString());
    }
}