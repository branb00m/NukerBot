using DSharpPlus;

namespace NukerBot.src.Core.Delegating;

public class StatusChangedEventArgs(ulong instanceID, InstanceCodes status, DiscordClient client) : EventArgs
{
    public ulong InstanceID { get; set; } = instanceID;
    public InstanceCodes Status { get; } = status;
    public DiscordClient Client { get; private set; } = client;
}

public class LastActionEventArgs(ulong instanceID, DateTime time, DiscordClient client) {
    public ulong InstanceID { get; set; } = instanceID;
    public DateTime Time { get; } = time;
    public DiscordClient Client { get; private set; } = client;
}