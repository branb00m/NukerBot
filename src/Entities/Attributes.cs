namespace NukerBot.src.Entities;

/// <summary>
/// `DiscordEventAttribute`. Used to essentially "mark" classes as a registerable `DiscordClient` event
/// Classes marked with this attribute must match the target event name
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class DiscordEventAttribute : Attribute;

/// <summary>
/// `CommandsNextEventAttribute`. Used to essentially "mark" classes as a registerable `CommandsNextExtension` event
/// Classes marked with this attribute must match the target event name
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class CommandsNextEventAttribute : Attribute;
