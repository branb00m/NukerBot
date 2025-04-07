using System.Net;
using Microsoft.Extensions.Logging;
using NukerBot.src.Utils;

namespace NukerBot.src.Core.Strategies.Proxies;

public sealed class ProxyManager(Config config)
{
    public IReadOnlyList<WebProxy> Proxies => proxies;
    private readonly List<WebProxy> proxies = [];

    private readonly ILogger logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ProxyManager>();

    private readonly Config config = config;

    public async Task GetProxies(string filePath = "")
    {
        int maxSize = config.Nuking.Options.Delegation.MaxSize;
        string[] selectedProxies = [];

        if (!GeneralUtils.IsValidFileExtension(filePath, ProxyUtils.FilePattern))
        {
            logger.LogWarning("Invalid file extension. Only accepted file extension is txt. Switching to default proxies");

            selectedProxies = config.Nuking.Options.Delegation.Proxies;
        }
        else
        {
            try
        {
            string[] fileProxies = await GetProxyAddressesAsync(filePath);

            if (fileProxies.Length < maxSize)
            {
                logger.LogWarning("Not enough custom proxies found in file. Using default proxies.");
                selectedProxies = config.Nuking.Options.Delegation.Proxies;
            }
            else
            {
                logger.LogInformation("Using custom proxies from file.");
                selectedProxies = fileProxies;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error reading proxies from file. Using default proxies instead.");
            selectedProxies = config.Nuking.Options.Delegation.Proxies;
        }
        }

        proxies.Clear();

        int added = 0;

        HashSet<string> seen = [];

        foreach (string proxy in selectedProxies)
        {
            if (added >= maxSize)
            {
                break;
            }

            if (string.IsNullOrWhiteSpace(proxy) || !seen.Add(proxy))
            {
                continue;
            }

            var parsed = ParseProxy(proxy);
            if (parsed is not null)
            {
                proxies.Add(parsed);
                added++;
            }
            else
            {
                logger.LogWarning("Skipping invalid proxy: {Proxy}", proxy);
            }
        }

        logger.LogInformation("Loaded {Count} proxies", added);
    }

    private WebProxy? ParseProxy(string proxyAddress)
    {
        if (string.IsNullOrWhiteSpace(proxyAddress))
        {
            return null;
        }

        var parts = proxyAddress.Split(':');

        if (parts.Length < 2)
        {
            return null;
        }

        if(!int.TryParse(parts[1], out int port)) {
            return null;
        }

        WebProxy proxy = new(parts[0], port);

        if (parts.Length == 4)
        {
            proxy.Credentials = new NetworkCredential(parts[2], parts[3]);
        }

        if (proxies.Contains(proxy))
        {
            return null;
        }

        return proxy;
    }

    private static async Task<string[]> GetProxyAddresses(string @filePath)
    {
        if(!File.Exists(filePath)) {
            throw new FileNotFoundException("File not found", filePath);
        }

        var lines = await File.ReadAllLinesAsync(filePath);

        return [.. lines
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()];
    }
}
