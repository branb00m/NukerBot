using System.Text.RegularExpressions;

namespace NukerBot.src.Utils;

public static class DelegationUtils
{
    public readonly static string TokenPattern = @"/[\w-]{24}\.[\w-]{6}\.[\w-]{27}/";
    public readonly static string InviteLink = @"(?:discord(?:app)?\.com/invite/|discord\.gg/)(\w+)";

    public static string ValidateToken(string token)
    {
        Match match = Regex.Match(token, TokenPattern, RegexOptions.IgnoreCase);

        // Prevents connections from being made if the provided string has an invalid regex pattern

        if (!match.Success)
        {
            throw new Exception($"Token provided in {nameof(token)} is invalid");
        }

        return match.Value;
    }

    public static string ValidateInvite(string inviteURL)
    {
        Match match = Regex.Match(inviteURL, InviteLink, RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            throw new Exception($"{inviteURL} is not a valid invite");
        }

        return match.Value;
    }
}