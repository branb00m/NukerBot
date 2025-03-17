using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NukerBot.src.Extensions;

public static partial class StringExtensions
{
    /// <summary>
    /// Masks a string by creating a new string with the length of the original string
    /// Since strings in C# are immutable, we had to solve for that by making a new string with the exact same length
    /// of the original string
    /// It also converts it to a codeblock
    /// </summary>
    /// <param name="value"> The original string </param>
    /// <param name="mask"> The character to mask the characters with </param>
    /// <returns> The masked string </returns>
    public static string MaskString(this string value, char mask = '*')
    {
        if (string.IsNullOrEmpty(value))
            return "empty"; // if string is empty, return "empty" as a string

        return '`' + new string(mask, value.Length) + '`'; // you know what this is
    }

    /// <summary>
    /// Capitalizes letters when necessary. Since this isn't built-in and requires a different library, I made my own equivalent with other utilities
    /// </summary>
    /// <param name="input"></param>
    /// <returns>The new string</returns>
    public static string CapitalizeString(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        input = input.Trim();

        StringBuilder builder = new(input);
        bool capitalizeNext = true;

        for (int i = 0; i < builder.Length; i++)
        {
            char character = builder[i];
            if (character == '.')
            {
                capitalizeNext = true;
            }
            else if (capitalizeNext && char.IsLetter(character))
            {
                builder[i] = char.ToUpper(character);

                capitalizeNext = false;
            }
        }

        return builder.ToString();
    }
}
