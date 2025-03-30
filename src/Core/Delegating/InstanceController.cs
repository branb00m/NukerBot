using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using NukerBot.src.Utils;

namespace NukerBot.src.Core.Delegating;

[Route("bots")]
[ApiController]
public sealed class InstanceController : ControllerBase
{
    private static readonly ConcurrentDictionary<ulong, BotInstance> _instances = new();

    [HttpPost("{ID}/start")]
    public async Task<IActionResult> StartBotAsync(ulong ID)
    {
        if (!_instances.TryGetValue(ID, out var bot))
        {
            return NotFound($"Instance {ID} not found");
        }

        await bot.StartAsync();
        return await Task.FromResult(Ok($"Instance {bot.ID} started"));
    }

    [HttpPost("{ID}/stop")]
    public async Task<IActionResult> StopBotAsync(ulong ID)
    {
        if (!_instances.TryGetValue(ID, out var bot))
        {
            return NotFound($"Instance {ID} not found");
        }

        await bot.StopAsync();
        return await Task.FromResult(Ok($"Instance {bot?.ID} stopped"));
    }

    [HttpPost("{ID}/restart")]
    public async Task<IActionResult> RestartBotAsync(ulong ID)
    {
        if (!_instances.TryGetValue(ID, out var bot))
        {
            return NotFound($"Instance {ID} not found");
        }

        await bot.RestartAsync();
        return await Task.FromResult(Ok($"Instance {bot.ID} restarted"));
    }

    [HttpPost("{ID}/pause")]
    public async Task<IActionResult> PauseBotAsync(ulong ID)
    {
        if (!_instances.TryGetValue(ID, out var bot))
        {
            return NotFound($"Instance {ID} not found");
        }

        await bot.PauseAsync();
        return await Task.FromResult(Ok($"Instance {bot.ID} restarted"));
    }
}

internal partial class BotInstance
{
    public ulong ID { get; }

    private readonly InstanceHTTPClient _httpClient;

    public InstanceCodes Status { get; private set; } = InstanceCodes.Offline;

    internal BotInstance(ulong ID, string token)
    {
        this.ID = ID;

        token = ReadToken(token).Value;

        _httpClient = new(token);
    }

    public async Task StartAsync() => await ChangeStatusAsync(InstanceCodes.Online);

    public async Task StopAsync() => await ChangeStatusAsync(InstanceCodes.Offline);

    public async Task PauseAsync() => await ChangeStatusAsync(InstanceCodes.Idle);

    public async Task RestartAsync() => await ChangeStatusAsync(InstanceCodes.Restarted);

    private static Match ReadToken(string token)
    {
        Match match = Regex.Match(token, DelegationUtils._tokenPattern, RegexOptions.IgnoreCase);

        // Prevents connections from being made if the provided string has an invalid regex pattern

        if (!match.Success)
        {
            throw new Exception($"{token} is an invalid token");
        }

        return match;
    }
}

internal partial class BotInstance
{
    public event EventHandler<InstanceCodes>? OnStatusChanged;
    private async Task ChangeStatusAsync(InstanceCodes newStatus)
    {
        Status = newStatus;

        OnStatusChanged?.Invoke(this, newStatus);

        await Task.CompletedTask;
    }
}