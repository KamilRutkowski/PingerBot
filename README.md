# PingerBot
Discord Bot in C#. It will support custom text commands, time alerts, possibly custom voice-chat sounds.

#Setting up bot
You have to add a configuration.json in Pinger directory. It has to be in format of:
```
{
  "BotToken": "<YOUR BOT TOKEN HERE>",
  "Commands": [
  ]
}
```
Where Commands are user defined commands which are automaticaly saved in configuration.json which is located in your build directory.
After this you should add this file to your VS project (make it copy/copy if newer if you want).
