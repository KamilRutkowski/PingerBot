using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reactive;
using System.Reactive.Linq;

namespace Pinger
{
    public class PingerBot
    {
        DiscordSocketClient _client;
        DiscordSocketConfig _config;
        CommandService _commandService;
        IDisposable _commandUpdatedCallback;
        

        public PingerBot(DiscordSocketConfig conf, CommandService comService)
        {
            _config = conf;
            _client = new DiscordSocketClient(_config);
            _client.Log += log;
            _commandService = comService;
        }
        
        ~PingerBot()
        {
            _commandUpdatedCallback.Dispose();
            _client.Dispose();            
        }

        private Task log(LogMessage msg)
        {
            Console.WriteLine(msg);
            return Task.FromResult(1);
        }

        public async Task MainRoutine()
        {
            await InstallTypeReaders();
            await SetCommands();

            ObserveCommandsUpdates();
            await InstallCommandHandler();

            await _client.LoginAsync(TokenType.Bot, GlobalConfig.BotToken);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private void ObserveCommandsUpdates()
        {
            _commandUpdatedCallback = GlobalConfig.CommandsUpdated.Subscribe(async _ => await SetCommands());
        }

        private async Task InstallTypeReaders()
        {
            _commandService.AddTypeReader<string>(new StringTypeReader());
        }

        private async Task SetCommands()
        {
            await _commandService.CreateModuleAsync("", module =>
            {                
                GlobalConfig.Commands.ForEach(command =>
                {
                    module.AddCommand(command.Name, SendChannelMessage, cmd => {  });
                });
            });
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private Task SendChannelMessage(ICommandContext context, object[] parameters, IServiceProvider arg3, CommandInfo command)
        {
            return context.Channel.SendMessageAsync(GlobalConfig.Commands.First(c => c.Name == command.Name).Response);
        }       

        private async Task InstallCommandHandler()
        {
            _client.MessageReceived += HandleCommand;
        }

        private async Task HandleCommand(SocketMessage mes)
        {
            var message = mes as SocketUserMessage;
            if (message == null)
                return;

            var prefixPosition = 0;

            if (!(message.HasCharPrefix('!', ref prefixPosition) || message.HasMentionPrefix(_client.CurrentUser, ref prefixPosition)))
                return;
            else
            {
                var commandContext = new CommandContext(_client, message);

                var result = await _commandService.ExecuteAsync(commandContext, prefixPosition);
                if (!result.IsSuccess)
                    await commandContext.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
}