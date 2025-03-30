using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace NukerBot.src.Core.Delegating;

public sealed class InstanceService
{
    public static async Task RunAsync(string[] args)
    {
        var host = GetHostBuilder(args).Build();

        await host.RunAsync();
    }

    private static IWebHostBuilder GetHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args)
        .UseStartup<StartupBase>();
}