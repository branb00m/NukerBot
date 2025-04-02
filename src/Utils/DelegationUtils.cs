using System.Text.RegularExpressions;

namespace NukerBot.src.Utils;

public static class DelegationUtils {
    public readonly static string _tokenPattern = @"/[\w-]{24}\.[\w-]{6}\.[\w-]{27}/";

    public static string ValidateToken(string token)
    {
        Match match = Regex.Match(token, _tokenPattern, RegexOptions.IgnoreCase);

        // Prevents connections from being made if the provided string has an invalid regex pattern

        if (!match.Success)
        {
            throw new Exception($"{token} is an invalid token");
        }

        return match.Value;
    }
}