# RexBot
 A silly and generally useless but expandable(for simple commands) discord bot. Credits to Pat for the glomp command idea and text options.
# Requirements
.NET Core 2.0
# Configuration
## RexBot.conf
  - ``Token``: The bots token.
  - ``AllowedCategories``: The custom command categories you choose to allow.
  - ``DisallowedCategories``: The custom command categories you choose not to allow.
  - ``AllowHardcoded``: Toggles 2 of the 4 of the hardcoded commands, flip and dieroll.
  - ``ControlChannel``: The channel that reload has to be used in, currently useless due to the state of the reload command.
  - ``Name``: The bots name that can be used in custom commands. **This does not set the bots nickname or username.**
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
Rexbot has a custom command system. Custom commands are json files that specify a commands category, name and output options. A commands category must be in ``AllowedCategories`` for it to load. Custom commands are loaded from the ``Commands`` folder within the ``Config`` folder.
```json
{
  "name": "testcommand",
  "Category": "testing",
  "Content": [
    "Text.",
    "More text.",
    "Even more text."
  ]
}
```
The above is an example of a custom command. There are multiple keywords that will be replaced by the bot you can use to for extra interaction with commands, they are as follows:
- ``_AUTHOR_``:The senders username.
- ``_CONTENT_``: Any text following the command name.
- ``_BOT_``: The bots name as set in the config.
- ``_IMAGE_``: Instead of replacing text, this will make the command attach an image in place of text. In order for this to work, you need to have a png file that is named the commands name. See the ``camera`` command for an example.
