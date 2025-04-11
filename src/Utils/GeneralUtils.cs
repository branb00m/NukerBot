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

    /// <summary>
    /// Better to add this to GeneralUtils instead of adding it to StringExtensions. Why?
    /// 
    /// Extensions aren't properly static, they require an instated string. Strings are immutable.
    /// 
    /// There's another way (again, it isn't properly static), by using String.Empty, but that only would create useless objects. The best way is by implementing this in the GeneralUtils class because then, it's properly static and doesn't require an instated string.
    /// </summary>
    /// <param name="random"></param>
    /// <param name="chars"></param>
    /// <param name="upper"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static string Randomize(Random random, string? chars = null, bool? upper = null, int size = 8)
    {
        if (size < 8)
        {
            throw new Exception($"{nameof(size)} cannot be less than 8 characters");
        }

        if (string.IsNullOrEmpty(chars))
        {
            chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        }
        else
        {
            chars = upper switch
            {
                true => chars.ToUpper(),
                false => chars.ToLower(),
                _ => chars
            };
        }

        char[] characters = new char[size];

        for (int i = 0; i < size; i++)
        {
            characters[i] = chars[random.Next(chars.Length)];
        }

        return new string(characters);
    }
}