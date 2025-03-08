const { GatewayIntentBits } = require('discord.js');

const intents = [
    GatewayIntentBits.Guilds,                       // Guild-related events
    GatewayIntentBits.GuildMembers,                 // Member updates (join, leave, role changes)
    GatewayIntentBits.GuildModeration,              // Moderation events (bans, unbans)
    GatewayIntentBits.GuildExpressions,             // Guild emojis + stickers
    GatewayIntentBits.GuildIntegrations,            // Integration updates
    GatewayIntentBits.GuildWebhooks,                // Webhook updates
    GatewayIntentBits.GuildInvites,                 // Invite creation and deletion
    GatewayIntentBits.GuildVoiceStates,             // Voice state updates
    GatewayIntentBits.GuildPresences,               // Presence updates (online/offline status)
    GatewayIntentBits.GuildMessages,                // Messages sent in guilds
    GatewayIntentBits.GuildMessageReactions,        // Reactions added to guild messages
    GatewayIntentBits.GuildMessageTyping,           // Typing indicators in guilds
    GatewayIntentBits.DirectMessages,               // Direct messages received
    GatewayIntentBits.DirectMessageReactions,       // Reactions in direct messages
    GatewayIntentBits.DirectMessageTyping,          // Typing indicators in direct messages
    GatewayIntentBits.MessageContent,               // Access to message content
    GatewayIntentBits.GuildScheduledEvents,         // Scheduled events in guilds
    GatewayIntentBits.AutoModerationConfiguration,  // Auto-moderation rule configurations
    GatewayIntentBits.AutoModerationExecution       // Auto-moderation actions
];

module.exports.intents = intents;