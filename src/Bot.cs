using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NukerBot.src.Entities;
using NukerBot.src.Utils;

namespace NukerBot.src;

public sealed class Bot
{
    public static readonly Config config = Config.Deserialize("src//config.jsonc");
    public readonly DiscordClient Client;

    private readonly CommandsNextExtension CommandsNext;
    private static readonly ServiceProvider services = new ServiceCollection()
        .AddSingleton(config)
        .BuildServiceProvider();

    public Bot()
    {
        Client = new(GetDiscordConfiguration(config));

        Client.UseInteractivity(GetInteractivityConfiguration());

        CommandsNext = Client.UseCommandsNext(GetCommandsNextConfiguration(config));
        CommandsNext.RegisterCommands(Assembly.GetExecutingAssembly());
    }

    public async Task Start()
    {
        BotEventHandler eventHandler = new(this);

        await eventHandler.RegisterEventsAsync();

        await Client.InitializeAsync();
        await Client.ConnectAsync();

        await Task.Delay(-1);
    }

    public static DiscordConfiguration GetDiscordConfiguration(Config config) => new()
    {
        Token = config.Client.Options.Token,
        TokenType = TokenType.Bot,
        Intents = DiscordIntents.All,
        MinimumLogLevel = LogLevel.Information,
        AutoReconnect = true
    };

    public static CommandsNextConfiguration GetCommandsNextConfiguration(Config config) => new()
    {
        StringPrefixes = config.Client.Options.Prefixes,
        CaseSensitive = false,
        Services = services
    };

    public static InteractivityConfiguration GetInteractivityConfiguration() => new() {
        PollBehaviour = DSharpPlus.Interactivity.Enums.PollBehaviour.DeleteEmojis,
        Timeout = TimeSpan.FromSeconds(30)
    };
}

public sealed class BotEventHandler
{
    private static readonly Assembly assembly = Assembly.GetExecutingAssembly();
    private readonly Bot bot;

    public BotEventHandler(Bot bot)
    {
        this.bot = bot ?? throw new ArgumentNullException(nameof(bot));
    }

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
