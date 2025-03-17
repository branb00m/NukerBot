using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using NukerBot.src.Extensions;

namespace NukerBot.src.Modules;

public sealed class TestingModule : BaseCommandModule {
    [Command("test")]
    public async Task GetTestAsync(CommandContext context, [RemainingText] string text) {
        var capitalized = text.CapitalizeString();

        await context.RespondAsync(capitalized);
    }
}