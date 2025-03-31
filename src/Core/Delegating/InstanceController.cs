using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

namespace NukerBot.src.Core.Delegating;

[ApiController]
[Route("bots")]
public sealed class InstanceController : ControllerBase
{
    private static readonly ConcurrentDictionary<ulong, BotInstance> _instances = new();

    [HttpPost("start/{ID}")]
    public async Task<IActionResult> StartBotAsync(ulong ID)
    {
        if (!_instances.TryGetValue(ID, out var bot))
        {
            return NotFound($"Instance {ID} not found");
        }

        await bot.StartAsync();
        return await Task.FromResult(Ok($"Instance {bot.ID} started"));
    }

    [HttpPost("stop/{ID}")]
    public async Task<IActionResult> StopBotAsync(ulong ID)
    {
        if (!_instances.TryGetValue(ID, out var bot))
        {
            return NotFound($"Instance {ID} not found");
        }

        await bot.StopAsync();
        return await Task.FromResult(Ok($"Instance {bot?.ID} stopped"));
    }

    [HttpPost("restart/{ID}")]
    public async Task<IActionResult> RestartBotAsync(ulong ID)
    {
        if (!_instances.TryGetValue(ID, out var bot))
        {
            return NotFound($"Instance {ID} not found");
        }

        await bot.RestartAsync();
        return await Task.FromResult(Ok($"Instance {bot.ID} restarted"));
    }

    [HttpPost("pause/{ID}")]
    public async Task<IActionResult> PauseBotAsync(ulong ID)
    {
        if (!_instances.TryGetValue(ID, out var bot))
        {
            return NotFound($"Instance {ID} not found");
        }

        await bot.PauseAsync();
        return await Task.FromResult(Ok($"Instance {bot.ID} restarted"));
    }

    [HttpGet("status/{ID}")]
    public async Task<IActionResult> GetBotStatusAsync(ulong ID)
    {
        if (!_instances.TryGetValue(ID, out var bot))
        {
            return NotFound($"Instance {ID} not found");
        }

        return await Task.FromResult(Ok(bot));
    }

    [HttpGet("bots")]
    public async Task<IActionResult> GetBotsAsync()
    {
        var bots = _instances.ToList();

        return await Task.FromResult(Ok(bots));
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