using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace Pinger
{
    class Program
    {
        static void Main(string[] args)
        {
            PingerBot bot = new PingerBot(new DiscordSocketConfig(), new CommandService());
            bot.MainRoutine().GetAwaiter().GetResult();
        }
    }
}
