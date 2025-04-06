using System.Text;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;

namespace NukerBot.src.Core;

public sealed partial class BotHelpFormatter : BaseHelpFormatter
{
    public DiscordEmbedBuilder EmbedBuilder { get; }

    private readonly CommandContext ctx;

    private Command? command;

    public BotHelpFormatter(CommandContext ctx) : base(ctx)
    {
        this.ctx = ctx;

        EmbedBuilder = new()
        {
            Color = EmbedColor
        };
    }

    public override BaseHelpFormatter WithCommand(Command command)
    {
        this.command = command;

        FormatDescription(command);

        FormatAliases(command);
        FormatCommand(command);
        FormatOverloads(command);

        return this;
    }

    public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
    {
        foreach (var cmd in subcommands.DistinctBy(c => c.Name))
        {
            EmbedBuilder.AddField($"{ctx.Prefix}{cmd.QualifiedName}", $"`{cmd.Description ?? "no help provided..."}`");
        }

        return this;
    }

    public override CommandHelpMessage Build()
    {
        if (command is null)
        {
            EmbedBuilder.WithDescription("showing all available commands");
            EmbedBuilder.WithFooter("this help command is your friend. this will aid you in whatever you're about to do", ctx.Client.CurrentUser.AvatarUrl);
        }
        else
        {
            EmbedBuilder.WithFooter("'[]' signifies command arguments. if your command has those braces, you need to specify arguments for that specific command");
        }

        EmbedBuilder.WithThumbnail(ctx.Client.CurrentUser.AvatarUrl);

        return new CommandHelpMessage(embed: EmbedBuilder.Build());
    }
}

public sealed partial class BotHelpFormatter
{
    public static DiscordColor EmbedColor => new(0x6354f0);

    public void FormatCommand(Command command)
    {
        EmbedBuilder.Title = command.QualifiedName;
    }

    private void FormatDescription(Command command)
    {
        StringBuilder builder = new();

        builder.AppendLine((command.Description is null || command.Description.Length == 0) ? "`no help provided...`" : $"`{command.Description}`");

        EmbedBuilder.WithDescription(builder.ToString());
    }

    private void FormatOverloads(Command command)
    {
        StringBuilder builder = new();

        if (command.Overloads.Count > 0)
        {
            foreach (CommandOverload? overload in command.Overloads.OrderByDescending(x => x.Priority))
            {
                string arguments;

                if (overload.Arguments.Count > 0)
                {
                    arguments = string.Join(' ', overload.Arguments.Select(a => $"`[{a.Name}]`"));
                }
                else
                {
                    arguments = "`no arguments found...`";
                }

                builder.AppendLine($"`{ctx.Prefix}{command.QualifiedName}` {arguments}");
            }
        }

        EmbedBuilder.AddField("arguments", builder.ToString().Trim(), false);
    }

    private void FormatAliases(Command command)
    {
        EmbedBuilder.AddField("aliases", command.Aliases.Count > 0 ? string.Join(", ", command.Aliases.OrderByDescending(x => $"`{x}`")) : "`no aliases found...`", false);
    }
}
