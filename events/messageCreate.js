const config = require("../config.json");

module.exports = {
    name: "messageCreate",
    once: false,
    async execute(message) {
        if (!message.content.startsWith(config.prefix)) return;

        if(message.author.id !== config.ownerid) return message.channel.send("You are not the owner!");

        const args = message.content.slice(config.prefix.length).trim().split(/ +/g);
        const command = args.shift().toLowerCase();

        if (!message.client.commands.has(command)) return;

        try {
            message.client.commands.get(command).execute(message, args);
        } catch (error) {
            message.channel.send(`\`${error}\``);
        }
    }
}