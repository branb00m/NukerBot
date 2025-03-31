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

        token = ReadToken(token).Value;

        _httpClient = new(token);
    }

    public async Task StartAsync() => await ChangeStatusAsync(InstanceCodes.Online);

    public async Task StopAsync() => await ChangeStatusAsync(InstanceCodes.Offline);

    public async Task PauseAsync() => await ChangeStatusAsync(InstanceCodes.Idle);

    public async Task RestartAsync() => await ChangeStatusAsync(InstanceCodes.Restarted);

    private static Match ReadToken(string token)
    {
        Match match = Regex.Match(token, DelegationUtils._tokenPattern, RegexOptions.IgnoreCase);

        // Prevents connections from being made if the provided string has an invalid regex pattern

        if (!match.Success)
        {
            throw new Exception($"{token} is an invalid token");
        }

        return match;
    }
}
