using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using NukerBot.src.Core.Entities;
using NukerBot.src.Utils;

namespace NukerBot.src.Extensions;

public static partial class DiscordClientExtensions
{
    public static async Task RegisterEventsAsync(this DiscordClient client)
    {
        foreach (var eventType in GetBotEvents<DiscordEventAttribute>())
        {
            await RegisterEventAsync(client, eventType);
        }

        foreach (var eventType in GetBotEvents<CommandsNextEventAttribute>())
        {
            await RegisterEventAsync(client.GetCommandsNext(), eventType);
        }

        await Task.CompletedTask;
    }

    private static async Task RegisterEventAsync(object target, Type eventType)
    {
        var eventInfo = target.GetType().GetEvent(eventType.Name);
        if (eventInfo == null)
        {
            Console.WriteLine($"{eventType.Name} is not a valid event on {target.GetType().Name}");
            return;
        }

        var method = eventType.GetMethod("MainAsync");
        if (method == null)
        {
            Console.WriteLine($"{eventType.Name} does not contain a 'MainAsync' method");
            return;
        }

        if (eventInfo.EventHandlerType == null)
        {
            Console.WriteLine($"{eventInfo.Name} EventHandlerType is null");
            return;
        }

        try
        {
            var delegateInstance = method.CreateDelegate(eventInfo.EventHandlerType);
            eventInfo.AddEventHandler(target, delegateInstance);

            Console.WriteLine($"{eventInfo.Name} initialized successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error initializing {eventInfo.Name}: {ex}");
        }

        await Task.CompletedTask;
    }

    private static IEnumerable<Type> GetBotEvents<TAttribute>() where TAttribute : Attribute =>
        Assembly.GetExecutingAssembly().GetTypes()
            .Where(static type => GeneralUtils.HasEventAttribute<TAttribute>(type) && type.GetMethod("MainAsync") is not null)
            .Select(static x => x);
}