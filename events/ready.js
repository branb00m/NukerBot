const {ActivityType} = require('discord.js');

module.exports = {
    name: "ready",
    once: true,
    execute(nukerBot) {
        console.log(nukerBot.user.id);

        nukerBot.user.setPresence({activities: [{ name: 'lol', type: ActivityType.Watching }], status: 'invisible'})
        nukerBot.guilds.cache.forEach(async guild => {
            if (guild.members.me.roles.cache.find(role => role.permissions.has("Administrator"))) {
                console.log(`Bot has permissions in ${guild.name}`);
            } else {
                console.log(`Bot has no permissions in ${guild.name}`);
            }
        });
    }
}