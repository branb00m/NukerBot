using System.Text.RegularExpressions;
using NukerBot.src.Utils;

namespace NukerBot.src.Core.Delegating;

internal partial class BotInstance
{
    public ulong ID { get; }

    private readonly InstanceHTTPClient _httpClient;

    public InstanceCodes Status { get; private set; } = InstanceCodes.Offline;

    internal BotInstance(ulong ID, string token)
    {
        this.ID = ID;

        token = DelegationUtils.ValidateToken(token);

        _httpClient = new(token);
    }

    public async Task StartAsync() => await ChangeStatusAsync(InstanceCodes.Online);

    public async Task StopAsync() => await ChangeStatusAsync(InstanceCodes.Offline);

    public async Task PauseAsync() => await ChangeStatusAsync(InstanceCodes.Idle);

    public async Task RestartAsync() => await ChangeStatusAsync(InstanceCodes.Restarted);
}

internal partial class BotInstance
{
    public event EventHandler<InstanceCodes>? OnStatusChanged;

    private async Task ChangeStatusAsync(InstanceCodes newStatus)
    {
        Status = newStatus;

        OnStatusChanged?.Invoke(this, newStatus);

        await Task.CompletedTask;
    }
}