# RexBot

A silly and generally useless but expandable(for simple commands) discord bot.
# Requirements
.NET Core 2.0
# Configuration
## RexBot.conf
  - ``key``: the bot API key you got from discord.
  - ``AllowedCategories``: The custom command categories you choose to allow.
  - ``DisallowedCategories``: The custom command categories you choose not to allow.
  - ``AllowHardcoded``: Toggles 2 of the 4 of the hardcoded commands, flip and dieroll.
  - ``ControlChannel``: The channel that reload has to be used in, currently useless due to the state of the reload command.
  - ``"Name"``: The bots name that can be used in custom commands **This does not set the bots nickname or actual username.**
  - ``Prefix``: The prefix commands should start with.
- ``DisabledCommands``: Allows commands to disabled individualy, a more fine grained alternative to ``DisallowedCategories`` and ``AllowHardcoded``.
## Subs.conf
  Contains the characters use for subsitutions in the ``flip`` command in following format. 
  ```
  original character|new character
  original character|new character
  original character|new character
  ```
# Hardcoded Commands
- ``list``: Lists the available commands.
- ``flip``: Subsitutes text with alternative characters to make it look upside down.
- ``dieroll``: Rolls a die with the specified ammount of sides,
- ``reload``: Currently not working, reloads the custom command files.
# Custom Commands
Rexbot has a custom command system. Custom commands are json files that specify a commands category, name and output options. A commands category must be in ``AllowedCategories`` for it to load.
```json
{
  "name": "glomp",
  "Category": "silly",
  "Content": [
    "*_USERNAME_ glomps _INPUT_ through a wall!*",
    "*_USERNAME_ ruptures _INPUT_'s spleen with a powerful glomp!*",
    "*_USERNAME_ tries to glomp _INPUT_ but accidentally dives in front of a car.*",
    "*_USERNAME_ glomps at breakneck speed, breaking _INPUT_'s neck*"
  ]
}
```
The above is an example of a custom command. There are multiple keywords that will be replace by the bot you can use to get use interaction for commands, they are as follows:
- ``_USERNAME_`` is replaced by the senders username.
- ``_INPUT_`` is replace by whatever follows the command.
- ``_BOTNAME_`` is replace by the bots name as set in the config.
