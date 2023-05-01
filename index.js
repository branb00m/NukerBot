const config = require("./config.json");

const {Client, Collection, IntentsBitField, ActivityType} = require("discord.js");
const path = require("path");
const fs = require("fs");

const intents = new IntentsBitField([3276799]);

let nukerBot = new Client({intents: intents});

nukerBot.commands = new Collection();

const commandFiles = fs.readdirSync(path.join(__dirname, 'commands')).filter(file => file.endsWith(".js"));
const eventFiles = fs.readdirSync(path.join(__dirname, 'events')).filter(file => file.endsWith(".js"));

for (const file of commandFiles) {
    const command = require(`./commands/${file}`);
    nukerBot.commands.set(command.name, command);

    console.log(`Set ${file}`);
}


for (const file of eventFiles) {
    const event = require(`./events/${file}`);
    if (event.once) {
        nukerBot.once(event.name, (...args) => event.execute(...args, nukerBot));
    } else {
        nukerBot.on(event.name, (...args) => event.execute(...args, nukerBot));
    }
}

nukerBot.login(config.token).then(() => {
    console.log('Successfully logged in!');
});
