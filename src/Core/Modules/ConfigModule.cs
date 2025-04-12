using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace NukerBot.src.Core.Modules;

[Group("config")]
public sealed class ConfigModule : BaseCommandModule {
    public required Config ConfigService { get; set; }

    [GroupCommand]
    public async Task ConfigAsync(CommandContext context) {
        await context.Channel.SendMessageAsync("lol");
    }

    [Command("list")]
    public async Task ListConfigAsync(CommandContext context) {
    }
}