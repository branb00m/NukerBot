using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using NukerBot.src.Core.Entities;
using NukerBot.src.Extensions;
using NukerBot.src.Utils;

namespace NukerBot.src.Core.Modules;

public sealed class ReplicationModule : BaseCommandModule {
    public required Config Config { get; set; }
    public required Random Random { get; set; }

    [Command("replicate")]
    [Description("This command mimics a popular bot's name, avatar, status and bio for infiltration purposes")]
    [Aliases("mimic")]
    [HasAccepted]
    public async Task ReplicateAsync(CommandContext context, string? name = null)
    {
        if (name == null)
        {
            ImpersonationBot random = GeneralUtils.GetRandomItem(Random, Config.Nuking.Options.ImpersonationBots);

            var impersonationBot = await ImpersonationBot.ToDiscordUserAsync(random, context.Client);

            if (impersonationBot.Username == context.Client.CurrentUser.Username)
            {
                await context.RespondAsync($"`the chosen bot already is '{context.Client.CurrentUser.Username}'`");

                return;
            }

            byte[] avatar = await DiscordUtils.DownloadAvatarAsync(impersonationBot);
            using var stream = new MemoryStream(avatar);

            await context.Client.UpdateCurrentUserAsync(impersonationBot.Username, stream);
            await context.RespondAsync($"`successfully replicated {impersonationBot.Username}`");

            await stream.DisposeAsync();

            return;
        }
        else
        {
            ImpersonationBot? bot = Config.Nuking.Options.ImpersonationBots.Find(x => x.Name == name.CapitalizeString());

            if (bot is null)
            {
                await context.RespondAsync($"`{name} is not a valid bot to impersonate`");
                return;
            }

            var impersonationBot = await ImpersonationBot.ToDiscordUserAsync(bot, context.Client);

            if (impersonationBot.Username == context.Client.CurrentUser.Username)
            {
                await context.RespondAsync($"`the chosen bot already is '{context.Client.CurrentUser.Username}'`");

                return;
            }

            byte[] avatar = await DiscordUtils.DownloadAvatarAsync(impersonationBot);
            using var stream = new MemoryStream(avatar);

            await context.Client.UpdateCurrentUserAsync(impersonationBot.Username, stream);
            await context.RespondAsync($"`successfully replicated {impersonationBot.Username}`");

            await stream.DisposeAsync();

            return;
        }
    }
}