using DSharpPlus;
using NukerBot.src.Core.Strategies.Proxies;

namespace NukerBot.src.Core.Delegating;

public sealed partial class DelegatedNetworkInstance : object
{
    public ulong ID { get; }
    public InstanceCodes Status { get; private set; } = InstanceCodes.Offline;

    private readonly ProxyManager manager;
    private readonly DiscordClient client;
    private readonly DelegatedNetworkHTTPClient HTTPClient = new();

    internal DelegatedNetworkInstance(ulong botID, ProxyManager manager, DiscordClient client)
    {
        this.manager = manager;
        this.client = client;

        ID = botID;
    }

    public async Task StopAsync()
    {
        if (Status == InstanceCodes.Offline)
        {
            return;
        }

        Status = InstanceCodes.Offline;

        OnStatusChanged?.Invoke(this, new StatusChangedEventArgs(ID, Status, client));

        await Task.CompletedTask;
    }

    public async Task RestartAsync()
    {
        if (Status == InstanceCodes.Restarted)
        {
            return;
        }

        Status = InstanceCodes.Restarted;

        await Task.Delay(500);

        Status = InstanceCodes.Online;

        OnStatusChanged?.Invoke(this, new StatusChangedEventArgs(ID, Status, client));

        await Task.CompletedTask;
    }

    public async Task PauseAsync()
    {
        if (Status == InstanceCodes.Idle)
        {
            return;
        }

        Status = InstanceCodes.Idle;

        OnStatusChanged?.Invoke(this, new StatusChangedEventArgs(ID, Status, client));

        await Task.CompletedTask;
    }

    public async Task StartAsync()
    {
        if (Status == InstanceCodes.Online)
        {
            return;
        }

        Status = InstanceCodes.Online;

        OnStatusChanged?.Invoke(this, new StatusChangedEventArgs(ID, Status, client));

        await Task.CompletedTask;
    }
}

public sealed partial class DelegatedNetworkInstance : object
{
    public event EventHandler? OnStatusChanged;
    public event EventHandler? OnLastAction;

    async Task StatusChanged(InstanceCodes status, DiscordClient client)
    {
        OnStatusChanged?.Invoke(this, new StatusChangedEventArgs(ID, status, client));

        await Task.CompletedTask;
    }

    async Task LastAction(DateTime time) {
        await Task.CompletedTask;
    }
}