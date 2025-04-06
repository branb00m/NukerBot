using System.Reflection;
using System.Text.RegularExpressions;
using DSharpPlus.Entities;

namespace NukerBot.src.Utils;

public static class GeneralUtils
{
    public static bool HasEventAttribute<TEventAttribute>(Type type) where TEventAttribute : Attribute =>
        type.GetCustomAttribute<TEventAttribute>() is not null;

    public static TItem GetRandomItem<TItem>(Random random, List<TItem> items) =>
        items[random.Next(items.Count)];

    public static void GetTotalLines(string @directoryPath)
    {
        int totalLines = 0;

        foreach (var file in Directory.EnumerateFiles(directoryPath, "*.cs", SearchOption.AllDirectories))
        {
            try
            {
                string[] lines = File.ReadAllLines(file);
                totalLines += lines.Length;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file {file}: {ex.Message}");
            }
        }

        Console.WriteLine(totalLines);
    }

    public static bool IsValidFileExtension(string @filePath, string pattern)
    {
        if(string.IsNullOrWhiteSpace(filePath)) {
            throw new ArgumentNullException(nameof(filePath), "Cannot be null, empty or whitespace");
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(filePath);
        }

        return Regex.IsMatch(Path.GetFileName(@filePath), pattern);
    }
}