using System.Collections.Concurrent;
using DSharpPlus;
using Microsoft.Extensions.Logging;
using NukerBot.src.Core.Strategies.Proxies;

namespace NukerBot.src.Core.Delegating;

public sealed class DelegatedNetworkManager
{
    public IReadOnlyDictionary<ulong, DelegatedNetworkInstance> Instances => _instances;

    private readonly ILogger _logger;
    private readonly Config _config;
    private readonly ConcurrentDictionary<ulong, DelegatedNetworkInstance> _instances;

    private readonly ProxyManager _manager;

    public required DiscordClient Client { get; set; }

    public DelegatedNetworkManager(Config config, ProxyManager manager)
    {
        _config = config;

        _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<DelegatedNetworkManager>();
        _instances = new(Environment.ProcessorCount, _config.Nuking.Options.Delegation.MaxSize);

        _manager = manager;
    }

    public bool IsFull => _instances.Count >= _config.Nuking.Options.Delegation.MaxSize;

    public IEnumerable<DelegatedNetworkInstance> GetDelegatedNetworkInstances() => _instances.Values;

    public async Task<bool> AddInstanceAsync(ulong botID)
    {
        if (IsFull)
        {
            _logger.LogWarning("Maximum bot instance size reached");

            return await Task.FromResult(false);
        }

        if (_instances.ContainsKey(botID))
        {
            _logger.LogInformation("{botID} already exists", botID);

            return await Task.FromResult(false);
        }

        var bot = new DelegatedNetworkInstance(botID, _manager, Client);

        if (_instances.TryAdd(botID, bot))
        {
            _logger.LogInformation("Added instance {botID}", botID);

            return await Task.FromResult(true);
        }

        return await Task.FromResult(false);
    }

    public async Task<bool> StartBotAsync(ulong botID)
    {
        if (_instances.TryGetValue(botID, out var instance))
        {
            _logger.LogInformation("Started instance {botID}", botID);
            await instance.StartAsync();

            return await Task.FromResult(true);
        }

        return await Task.FromResult(false);
    }

    public async Task<bool> RestartBotAsync(ulong botID)
    {
        if (_instances.TryGetValue(botID, out var instance))
        {
            _logger.LogInformation("Restarted instance {botID}", botID);
            await instance.RestartAsync();

            return await Task.FromResult(true);
        }

        return await Task.FromResult(false);
    }

    public async Task<bool> StopBotAsync(ulong botID)
    {
        if (_instances.TryGetValue(botID, out var instance))
        {
            _logger.LogInformation("Started instance {botID}", botID);
            await instance.StopAsync();

            return await Task.FromResult(true);
        }

        return await Task.FromResult(false);
    }

    public async Task StopAllBotsAsync()
    {
        foreach (var instance in _instances.Values)
        {
            if (instance.Status == InstanceCodes.Offline)
            {
                continue;
            }

            await instance.StopAsync();
        }

        _logger.LogInformation("Stopped all online, idle or restarted instances");
    }
}