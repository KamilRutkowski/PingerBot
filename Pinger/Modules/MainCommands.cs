using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Pinger.Modules
{
    [Group("Command")]
    public class MainCommands: ModuleBase
    {
        [Command("Add")]
        public async Task AddCommand(string commandName, string response)
        {
            if (GlobalConfig.Commands.Where(c => c.Name == commandName).Any())
            {
                await Context.Channel.SendMessageAsync($"Command {commandName} already exists");
                return;
            }
            var updatedCommands = GlobalConfig.Commands;
            updatedCommands.Add(new CommandObject { Name = commandName, Response = response });
            GlobalConfig.Commands = updatedCommands;
            await Context.Channel.SendMessageAsync($"Command {commandName} added successfully");
        }

        [Command("Delete")]
        public async Task DeleteCommand(string commandName)
        {
            if (GlobalConfig.Commands.Where(c => c.Name == commandName).Any())
            {
                var updatedCommands = GlobalConfig.Commands;
                updatedCommands.Remove(GlobalConfig.Commands.First(c => c.Name == commandName));
                GlobalConfig.Commands = updatedCommands;
                await Context.Channel.SendMessageAsync($"Command {commandName} deleted successfully");
                return;
            }
            await Context.Channel.SendMessageAsync($"Command {commandName} did not exist");
        }

        [Command("Help")]
        public async Task HelpCommand()
        {
            List<string> buildInCommands = new List<string>
            {
                "Add - Adding new custom command (!Command Add <command name> <\"command response\">)",
                "Delete - Delete custom command (!Command Delete <command name>)",
                "Help - Shows help, you just called it :)!"
            };
            StringBuilder sb = new StringBuilder();
            sb.Append("Build in commands (call with !Command <command name>):\n");
            foreach(var bic in buildInCommands)
            {
                sb.Append($"{bic}\n");
            }
            await Context.Channel.SendMessageAsync(sb.ToString());
            sb.Clear();
            sb.Append("Custom commands:\n");
            foreach(var command in GlobalConfig.Commands)
            {
                sb.Append("!"+command.Name + "\n");
            }
            await Context.Channel.SendMessageAsync($"{sb.ToString()}");
        }
    }
}
