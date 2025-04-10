using DSharpPlus;
using DSharpPlus.AsyncEvents;

namespace NukerBot.src.Core.Delegating;

public class StatusChangedEventArgs(ulong InstanceID, InstanceCodes status, DiscordClient client) : EventArgs
{
    public ulong InstanceID { get; set; } = InstanceID;
    public InstanceCodes Status { get; } = status;
    public DiscordClient Client { get; private set; } = client;
}
