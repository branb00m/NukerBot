module.exports = {
    name: "guildDelete",
    once: false,
    execute(guild) {
        console.log(`I was banned from ${guild.name}: ${guild.id}`);
    }
}