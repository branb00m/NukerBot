using DSharpPlus;
using NukerBot.src.Core.Strategies.Proxies;

namespace NukerBot.src.Core.Delegating;

public partial class DelegatedNetworkInstance : object
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

        await OnStatusChangedAsync();
        await Task.CompletedTask;
    }

    public async Task RestartAsync()
    {
        if (Status == InstanceCodes.Restarted)
        {
            return;
        }
        Status = InstanceCodes.Restarted;

        await OnStatusChangedAsync();
        await Task.Delay(500); // this line is literally for simulation. modify it if you want, you're a bozo if you do
        await OnStatusChangedAsync();

        Status = InstanceCodes.Online;


        await Task.CompletedTask;
    }

    public async Task PauseAsync()
    {
        if (Status == InstanceCodes.Idle)
        {
            return;
        }

        Status = InstanceCodes.Idle;

        await OnStatusChangedAsync();
        await Task.CompletedTask;
    }

    public async Task StartAsync()
    {
        if (Status == InstanceCodes.Online)
        {
            return;
        }

        Status = InstanceCodes.Online;

        await OnStatusChangedAsync();
        await Task.CompletedTask;
    }
}

public partial class DelegatedNetworkInstance : object
{
    public event OnStatusChanged? StatusChanged;
    public event OnLastAction? LastAction;

    public delegate Task OnStatusChanged(object sender, StatusChangedEventArgs args);
    public delegate Task OnLastAction(object sender, LastActionEventArgs args);

    protected virtual async Task OnStatusChangedAsync()
    {
        if (StatusChanged is not null)
        {
            await StatusChanged.Invoke(this, new StatusChangedEventArgs(ID, Status, client));
        }
    }

    protected virtual async Task OnLastActionAsync() {
        throw new NotImplementedException();
    }
}