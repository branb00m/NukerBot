module.exports = {
    name: "eval",
    description: "Evaluates code",
    execute(message, args) {
        try{eval(args.join(" "))}catch(error){message.channel.send(`\`${error}\``)}
    }
}