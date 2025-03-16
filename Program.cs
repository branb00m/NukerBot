using System.Reflection;
using NukerBot.src;
using NukerBot.src.Utils;

namespace NukerBot;

public class Program
{
    public async static Task Main(string[] args)
    {
        Bot bot = new();
        await bot.Start();
    }
}
