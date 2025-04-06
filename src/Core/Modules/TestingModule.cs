using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using NukerBot.src.Extensions;

namespace NukerBot.src.Core.Modules;

public sealed class TestingModule : BaseCommandModule
{
    [Command("test")]
    public async Task GetTestAsync(CommandContext context, [RemainingText] string text)
    {
        var flipped = text.FlipString();

        await context.RespondAsync(flipped);
    }
}