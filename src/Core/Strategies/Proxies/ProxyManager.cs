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

    public WebProxy? GetProxy() => proxies.FirstOrDefault();

    public async Task GetProxies(string? filePath = null)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath), "Cannot be null or whitespace");


        int maxSize = config.Nuking.Options.Delegation.MaxSize;
        string[] selectedProxies;

        if (!GeneralUtils.IsValidFileExtension(filePath, ProxyUtils.FilePattern))
        {
            logger.LogWarning("Invalid file extension. Using default proxies");
            selectedProxies = config.Nuking.Options.Delegation.Proxies;
        }
        else
        {
            string[] proxiesFromFile = await File.ReadAllLinesAsync(filePath);

            if (proxiesFromFile.Length < maxSize)
            {
                logger.LogWarning("Not enough custom proxies. Using default proxies");
                selectedProxies = config.Nuking.Options.Delegation.Proxies;
            }
            else
            {
                logger.LogInformation("Using custom proxies from file");
                selectedProxies = proxiesFromFile;
            }
        }

        proxies.Clear();

        int added = 0;
        foreach (string proxy in selectedProxies)
        {
            if (added >= maxSize)
                break;

            var parsed = ParseProxy(proxy);
            if (parsed != null)
            {
                proxies.Add(parsed);
                added++;
            }
            else
            {
                logger.LogWarning("Skipping invalid proxy: {Proxy}", proxy);
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
