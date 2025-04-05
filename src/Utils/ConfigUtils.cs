using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using NukerBot.src.Core;

namespace NukerBot.src.Utils;

public static class ConfigUtils
{

    public static string CheckConfigPath(string @filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentNullException(nameof(filePath), "filePath cannot be null or empty");
        }

        if (!(IsSpecificPath(filePath, "src/Config/Default/config.jsonc") || IsSpecificPath(filePath, "src/Config/Custom/config.jsonc")))
        {
            throw new ArgumentException("Has to be a relative JSON file", nameof(filePath));
        }

        return filePath;
    }

    /// <summary>
    /// This method is basically redundant outside of `Config`. It's only used to determine if two paths are equal
    /// </summary>
    /// <param name="inputPath"></param>
    /// /// <param name="targetPath"></param>
    /// <returns></returns>
    public static bool IsSpecificPath(string inputPath, string targetPath)
    {
        if (!File.Exists(inputPath))
            return false;

        string normalizedInput = Path.GetFullPath(inputPath).TrimEnd(Path.DirectorySeparatorChar);
        string normalizedTarget = Path.GetFullPath(targetPath).TrimEnd(Path.DirectorySeparatorChar);

        if (targetPath.Contains('*') || targetPath.Contains('?'))
        {
            string regexPattern = '^' + Regex.Escape(normalizedTarget)
                .Replace(@"\", ".*")
                .Replace(@"\?", ".")
                + '$';

            return Regex.IsMatch(normalizedInput, regexPattern, RegexOptions.IgnoreCase);
        }

        return normalizedInput.Equals(normalizedTarget, StringComparison.OrdinalIgnoreCase);
    }

    private static object? GetDefaultValue(Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }

        if (type == typeof(string))
        {
            return string.Empty;
        }

        if (typeof(IEnumerable).IsAssignableFrom(type))
        {
            return Activator.CreateInstance(type);
        }

        return null;
    }
}