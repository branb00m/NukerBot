using System.Reflection;
using NukerBot.src.Core;

namespace NukerBot.src.Utils;

public static class ConfigUtils {
    public static void CheckConfig<TConfig>(TConfig config) where TConfig : Config {
        var type = typeof(TConfig);

        var properties = type.GetProperties(BindingFlags.Public);
        foreach(var property in properties) {
            
        }
    }
}