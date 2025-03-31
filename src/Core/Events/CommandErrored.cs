using System.Runtime.InteropServices;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using NukerBot.src.Core.Entities;

namespace NukerBot.src.Core.Events;

[CommandsNextEvent]
public static class CommandErrored
{
    public static async Task MainAsync(CommandsNextExtension commandsNext, CommandErrorEventArgs args)
    {
        var context = args.Context;
        if (args.Exception is ChecksFailedException checksFailedException)
        {
            foreach (var check in checksFailedException.FailedChecks)
            {
                switch (check)
                {
                    case IsOwnerAttribute:
                        await context.RespondAsync("`this command is restricted to only the owner`");
                        return;

                    case CooldownAttribute cooldown:
                        var cooldownTime = cooldown.GetRemainingCooldown(context);
                        await context.RespondAsync($"`you can use this command again in {cooldownTime:F1} seconds`");
                        return;

                    case HasAcceptedAttribute:
                        await context.RespondAsync("`this command requires you to accept the Accepted condition`");
                        return;

                    default:
                        await context.RespondAsync("`requirement check failed`");
                        return;
                }
            }
        }
        else if (args.Exception is CommandNotFoundException)
        {
            await context.RespondAsync("`that command does not exist`");
        }
        else if (args.Exception is ArgumentException || args.Exception is ArgumentNullException)
        {
            await context.RespondAsync(args.Exception.Message);
        }
        else
        {
            await context.RespondAsync($"`an unexpected error occurred while executing the command: {args.Exception.Message}`");
            Console.WriteLine($"`[ERROR] {args.Exception.Message}\n{args.Exception.StackTrace}`");
        }
    }
}