using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using NukerBot.src.Entities;

namespace NukerBot.src.Events;

[DiscordEvent]
public static class Ready {
    private readonly static DiscordActivity activity = new("lol", ActivityType.Streaming);

    public static async Task MainAsync(DiscordClient client, ReadyEventArgs args) {
        await client.UpdateStatusAsync(activity);

        Console.WriteLine($"Is this event name an AC/DC reference, {client.CurrentUser.Username}?");

        await Task.CompletedTask;
    }
}
