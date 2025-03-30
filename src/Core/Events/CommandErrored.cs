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
                    case RequireOwnerAttribute:
                        await context.RespondAsync("`this command is restricted to only the Hypotenuse gods`");
                        return;

                    case RequireGuildAttribute:
                        await context.RespondAsync("`this command can only be used in a server (not in DMs)`");
                        return;

                    case RequireDirectMessageAttribute:
                        await context.RespondAsync("`this command can only be used in direct messages`");
                        return;

                    case RequirePermissionsAttribute perms:
                        await context.RespondAsync($"`you require the following permissions: {perms.Permissions}`");
                        return;

                    case RequireRolesAttribute roles:
                        var roleMentions = roles.RoleNames.Select(r => $"`{r}`");
                        await context.RespondAsync($"`you need one of these roles: {string.Join(", ", roleMentions)}`");
                        return;

                    case RequireUserPermissionsAttribute userPerms:
                        await context.RespondAsync($"`you require {userPerms.Permissions} permissions to use this command`");
                        return;

                    case RequireBotPermissionsAttribute botPerms:
                        await context.RespondAsync($"`the bot needs {botPerms.Permissions} permissions to execute this command`");
                        return;

                    case CooldownAttribute cooldown:
                        var cooldownTime = cooldown.GetRemainingCooldown(context);
                        await context.RespondAsync($"`you can use this command again in {cooldownTime:F1} seconds`");
                        return;

                    case RequireNsfwAttribute:
                        await context.RespondAsync("`this command can only be used in NSFW channels`");
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