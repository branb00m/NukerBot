using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using NukerBot.src.Entities;

namespace NukerBot.src.Events;

[DiscordEvent]
public static class MessageReactionAdded {
    public static async Task MainAsync(DiscordClient client, MessageReactionAddEventArgs args) {
        var message = args.Message;

        var channel = args.Channel;
        var emoji = args.Emoji;
        var user = args.User;
        var guild = args.Guild;

        

        await Task.CompletedTask;
    }
}
