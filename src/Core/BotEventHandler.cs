using System.Reflection;
using DSharpPlus.CommandsNext;
using NukerBot.src.Core.Entities;

namespace NukerBot.src.Core;

[Obsolete("Declared obsolete because of DiscordClientExtensions")]
public sealed class BotEventHandler(Bot bot)
{
    private static readonly Assembly assembly = Assembly.GetExecutingAssembly();
    private readonly Bot bot = bot ?? throw new ArgumentNullException(nameof(bot));

    public async Task RegisterEventsAsync()
    {
        foreach (var eventType in GetBotEvents<DiscordEventAttribute>())
        {
            await RegisterEventAsync(bot.Client, eventType);
        }

        foreach (var eventType in GetBotEvents<CommandsNextEventAttribute>())
        {
            await RegisterEventAsync(bot.Client.GetCommandsNext(), eventType);
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

        var methodInfo = eventType.GetMethod("MainAsync");
        if (methodInfo == null)
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
            var delegateInstance = methodInfo.CreateDelegate(eventInfo.EventHandlerType);
            eventInfo.AddEventHandler(target, delegateInstance);

            Console.WriteLine($"{eventInfo.Name} initialized successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error initializing {eventInfo.Name}: {ex}");
        }

        await Task.CompletedTask;
    }

    private static IEnumerable<Type> GetBotEvents<TAttribute>() where TAttribute : Attribute => assembly.GetTypes()
        .Where(static type => type.GetCustomAttribute<TAttribute>() is not null && type.GetMethod("MainAsync") is not null)
        .Select(static x => x);
}
