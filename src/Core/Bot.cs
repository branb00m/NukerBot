using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NukerBot.src.Extensions;

namespace NukerBot.src.Core;

public sealed class Bot
{
    public static readonly Config Config = Config.Deserialize("src//Config//Default//config.jsonc");
    public static readonly Random Random = new();
    public readonly DiscordClient Client;

    private readonly CommandsNextExtension CommandsNext;
    private static readonly ServiceProvider services = new ServiceCollection()
        .AddSingleton(Config)
        .AddSingleton(Random)
        .BuildServiceProvider();

    public Bot()
    {
        Client = new(GetDiscordConfiguration(Config));

        Client.UseInteractivity(GetInteractivityConfiguration());

        CommandsNext = Client.UseCommandsNext(GetCommandsNextConfiguration(Config));
        CommandsNext.RegisterCommands(Assembly.GetExecutingAssembly());
    }

    public async Task Start()
    {
        await Client.RegisterEventsAsync();

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

    public static InteractivityConfiguration GetInteractivityConfiguration() => new()
    {
        PollBehaviour = DSharpPlus.Interactivity.Enums.PollBehaviour.DeleteEmojis,
        Timeout = TimeSpan.FromSeconds(30)
    };
}
