using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;
using NukerBot.src.Entities;
using System.Reflection;

namespace NukerBot.src.Utils;

public static class GeneralUtils
{
    public static bool HasCommandsEventAttribute(Type type) => type.GetCustomAttribute<CommandsNextEventAttribute>() is not null;
    public static bool HasDiscordEventAttribute(Type type) => type.GetCustomAttribute<DiscordEventAttribute>() is not null;

    public static string Capitalize(string input) {
        if (string.IsNullOrEmpty(input)) return input;

        return char.ToUpper(input[0]) + input[1..].ToLower();
    }
}
