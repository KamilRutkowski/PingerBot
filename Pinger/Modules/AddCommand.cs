using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Pinger.Modules
{
    [Group("AddCommand")]
    public class AddCommand: ModuleBase
    {
        [Command]
        public async Task Default(string commandName, string response)
        {
            if (GlobalConfig.Commands.Where(c => c.Name == commandName).Any())
            {
                await Context.Channel.SendMessageAsync($"Command {commandName} already exists");
                return;
            }
            var updatedCommands = GlobalConfig.Commands;
            updatedCommands.Add(new CommandObject { Name = commandName, Response = response });
            GlobalConfig.Commands = updatedCommands;
            await Context.Channel.SendMessageAsync($"Command {commandName} added successfuly");
        }
    }
}
