using System.Net;
using Microsoft.Extensions.Logging;
using NukerBot.src.Utils;

namespace NukerBot.src.Core.Strategies.Proxies;

public sealed class ProxyManager
{
    public IReadOnlyList<WebProxy> Proxies => proxies;
    private readonly List<WebProxy> proxies = [];

    private readonly ILogger logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ProxyManager>();

    private readonly Config config;

    public ProxyManager(Config config)
    {
        this.config = config;
    }

    public async Task GetProxiesFromFile(string? filePath = null)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath), "Cannot be null or whitespace");

        if (!GeneralUtils.IsValidFileExtension(filePath, ProxyUtils.FilePattern))
        {
            logger.LogWarning("Invalid file extension. Skipping file: {FilePath}", filePath);
            return;
        }

        string[] proxiesFromFile = await File.ReadAllLinesAsync(filePath);
        int requiredCount = config.Nuking.Options.Delegation.MaxSize;

        string[] selectedProxies;

        if (proxiesFromFile.Length < requiredCount)
        {
            logger.LogWarning("Not enough custom proxies found. Reverting to default proxies.");
            selectedProxies = config.Nuking.Options.Delegation.Proxies;
        }
        else
        {
            logger.LogInformation("Using custom proxies from file.");
            selectedProxies = proxiesFromFile;
        }

        proxies.Clear();

        int added = 0;
        foreach (string proxy in selectedProxies)
        {
            if (added >= requiredCount)
                break;

            var parsed = ParseProxy(proxy);
            if (parsed != null)
            {
                proxies.Add(parsed);
                added++;
            }
            else
            {
                logger.LogWarning("Skipping invalid proxy format: {Proxy}", proxy);
            }
        }
    }

    private static WebProxy? ParseProxy(string proxy)
    {
        var proxyParts = proxy.Split(':');
        if (proxyParts.Length < 2)
        {
            return null;
        }

        WebProxy webProxy = new(proxyParts[0], int.Parse(proxyParts[1]));

        if (proxyParts.Length == 4)
        {
            webProxy.Credentials = new NetworkCredential(proxyParts[2], proxyParts[3]);
        }

        return webProxy;
    }
}
