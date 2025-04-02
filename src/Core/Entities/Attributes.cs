using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace NukerBot.src.Core.Entities;

/// <summary>
/// `DiscordEventAttribute`. Used to essentially "mark" classes as a registerable `DiscordClient` event.
/// Classes marked with this attribute must match the target event name
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class DiscordEventAttribute : Attribute {
    
}

/// <summary>
/// `CommandsNextEventAttribute`. Used to essentially "mark" classes as a registerable `CommandsNextExtension` event.
/// Classes marked with this attribute must match the target event name
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class CommandsNextEventAttribute : Attribute {

}

/// <summary>
/// `HasAcceptedAttribute`. Determines if the author has accepted the terms and conditions of using this program.
///
/// Commands marked with this attribute generally propose that if you use those marked commands, it will result in your account being suspended. Use this at your own risk
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class HasAcceptedAttribute : CheckBaseAttribute
{
    public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
    {
        var config = ctx.Services.GetRequiredService<Config>();

        return Task.FromResult(config.Nuking.Options.Accepted);
    }
}

/// <summary>
/// `IsOwnerAttribute`. Used to mark commands as owner only
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class IsOwnerAttribute : CheckBaseAttribute
{
    public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
    {
        var config = ctx.Services.GetRequiredService<Config>();

        return Task.FromResult(config.Client.Options.OwnerIDs.Contains(ctx.User.Id));
    }
}
