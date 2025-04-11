using DSharpPlus;
using Microsoft.AspNetCore.Mvc;
using NukerBot.src.Core.Strategies.Proxies;

namespace NukerBot.src.Core.Delegating;


[ApiController]
[Route("/")]
public sealed partial class DelegatedNetworkController : ControllerBase
{
    private readonly DelegatedNetworkManager networkManager;

    public DelegatedNetworkController(Config config, ProxyManager proxyManager, DiscordClient client) {
        networkManager = new(config, proxyManager) {
            Client = client
        };
    }

    [HttpPost("start/{ID}")]
    public async Task<IActionResult> StartBotAsync(ulong ID)
    {
        if (!networkManager.Instances.TryGetValue(ID, out var bot))
        {
            return NotFound($"Instance {ID} not found");
        }

        await bot.StartAsync();

        return await Task.FromResult(Ok($"Instance {bot.ID} started"));
    }

    [HttpPost("stop/{ID}")]
    public async Task<IActionResult> StopBotAsync(ulong ID)
    {
        if (!networkManager.Instances.TryGetValue(ID, out var bot))
        {
            return NotFound($"Instance {ID} not found");
        }

        await bot.StopAsync();

        return await Task.FromResult(Ok($"Instance {bot?.ID} stopped"));
    }

    [HttpPost("restart/{ID}")]
    public async Task<IActionResult> RestartBotAsync(ulong ID)
    {
        if (!networkManager.Instances.TryGetValue(ID, out var bot))
        {
            return NotFound($"Instance {ID} not found");
        }

        await bot.RestartAsync();

        return await Task.FromResult(Ok($"Instance {bot.ID} restarted"));
    }

    [HttpPost("pause/{ID}")]
    public async Task<IActionResult> PauseBotAsync(ulong ID)
    {
        if (!networkManager.Instances.TryGetValue(ID, out var bot))
        {
            return NotFound($"Instance {ID} not found");
        }

        await bot.PauseAsync();

        return await Task.FromResult(Ok($"Instance {bot.ID} restarted"));
    }

    [HttpGet("status/{ID}")]
    public async Task<IActionResult> GetBotStatusAsync(ulong ID)
    {
        if (!networkManager.Instances.TryGetValue(ID, out var bot))
        {
            return NotFound($"Instance {ID} not found");
        }

        return await Task.FromResult(Ok(bot));
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetBotsAsync()
    {
        var bots = networkManager.GetDelegatedNetworkInstances().ToList();

        return await Task.FromResult(Ok(bots));
    }
}