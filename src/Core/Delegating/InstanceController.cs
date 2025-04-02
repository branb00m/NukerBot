using System.Collections.Concurrent;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using NukerBot.src.Utils;

namespace NukerBot.src.Core.Delegating;

[ApiController]
[Route("bots")]
public sealed partial class InstanceController : ControllerBase
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

    [HttpGet("list")]
    public async Task<IActionResult> GetBotsAsync()
    {
        var bots = _instances.ToList();

        return await Task.FromResult(Ok(bots));
    }
}

public sealed partial class InstanceController
{
    public async Task LoadInstancesAsync(string @tokensPath)
    {
        string path = CheckPath("tokens.txt");

        foreach(var token in await System.IO.File.ReadAllLinesAsync(path))
        {
            try {
                var validated = DelegationUtils.ValidateToken(token);
            } catch (Exception e) {
                Console.WriteLine(e.Message);

                continue;
            }
        }
    }

    private static string CheckPath(string tokensPath)
    {
        var fullPath = Path.GetFullPath(tokensPath).TrimEnd(Path.PathSeparator);

        if (string.IsNullOrEmpty(tokensPath) || !Path.Exists(fullPath))
        {
            throw new Exception($"{nameof(tokensPath)} cannot be null, empty or invalid");
        }

        return fullPath;
    }
}