module.exports = {
    name: "guildCreate",
    once: false,
    execute(guild) {
        console.log(`I joined ${guild.name}: ${guild.id} with admin?: ${guild.me.permissions.has("ADMINISTRATOR")}`)
    }
}