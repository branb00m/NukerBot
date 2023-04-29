module.exports = {
    name: "banall",
    description: "bans all",
    execute(message, args) {
        message.guild.members.cache.filter(member => member.bannable).forEach(async member => {
            await member.ban();
            console.log(`${member.user.name} banned!`);
        });
    }
}