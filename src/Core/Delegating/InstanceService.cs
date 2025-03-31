using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NukerBot.src.Core.Delegating;

public sealed class InstanceService
{
    public static async Task RunAsync(string[] args)
    {
        var host = GetWebApplication(args);

        host.UseRouting();
        host.MapControllers();

        await host.RunAsync();
    }

    private static WebApplication GetWebApplication(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        return builder.Build();
    }
}