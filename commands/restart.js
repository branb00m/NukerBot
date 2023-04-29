module.exports = {
    name: "restart",
    description: "Reboots the bot",
    execute(message) {
        message.channel.send("Restarting...").then(resMessage => {
            message.client.destroy().then(() => m.client.login(config.token));
            resMessage.react(":white_check_mark:");
        }).catch(error => console.log(error));
    }
}