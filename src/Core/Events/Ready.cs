using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using NukerBot.src.Core.Entities;

namespace NukerBot.src.Core.Events;

[DiscordEvent]
public static class Ready
{
    private readonly static DiscordActivity activity = new("lol", ActivityType.Streaming);

    public static async Task MainAsync(DiscordClient client, ReadyEventArgs args)
    {
        await client.UpdateStatusAsync(activity);

        Console.WriteLine($"Is this event name an AC/DC reference, {client.CurrentUser.Username} - {client.CurrentUser.Id}?");

        await Task.CompletedTask;
    }
}
