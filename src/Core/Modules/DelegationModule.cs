using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace NukerBot.src.Core.Modules;

public sealed class DelegationModule : BaseCommandModule
{

    [Command("activate")]
    [Aliases("enable")]
    [Description("This activates your bot's delegate instance, awaiting instructions")]
    public async Task ActivateDelegateBotAsync(CommandContext context, ulong botID)
    {
        await Task.CompletedTask;
    }

    [Command("activateall")]
    [Aliases("enableall")]
    [Description("This activates ALL your bot's delegate instances, awaiting instructions")]
    public async Task ActivateAllDelegateBotsAsync(CommandContext context)
    {
        await Task.CompletedTask;
    }

    [Command("deactivate")]
    [Aliases("disable")]
    [Description("This deactivates your bot's delegate instance")]
    public async Task DeactivateDelegateBotAsync(CommandContext context, ulong botID)
    {
        await Task.CompletedTask;
    }

    [Command("deactivateall")]
    [Aliases("disableall")]
    [Description("This deactivates ALL your bot's delegate instances")]
    public async Task DeactivateAllDelegateBotsAsync(CommandContext context)
    {
        await Task.CompletedTask;
    }
}