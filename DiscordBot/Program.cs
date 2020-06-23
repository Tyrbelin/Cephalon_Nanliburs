using System;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;

namespace DiscordBot
{
    /*
     client Id : 722040816016556072
     token : NzIyMDQwODE2MDE2NTU2MDcy.XudT_g.BBPo8Yj8u_JB9IcfNe1gCCkwMVo
     */
    public class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();
        private DiscordSocketClient _client;
        private CommandHandler _handler;
        public async Task MainAsync()
        {
            Localize.Init();
            using (var services = ConfigureServices())
            {
                _client = services.GetRequiredService<DiscordSocketClient>();
                _client.Log += Log;
                services.GetRequiredService<CommandService>().Log += Log;
                //  You can assign your bot token to a string, and pass that in to connect.
                //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
                //var token = Environment.GetEnvironmentVariable("BOT_TOKEN",EnvironmentVariableTarget.Machine);
                var token = Environment.GetEnvironmentVariable("BOT_TOKEN");
                // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
                // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
                // var token = File.ReadAllText("token.txt");
                // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

                await _client.LoginAsync(TokenType.Bot, token);
                await _client.StartAsync();
                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                // Block this task until the program is closed.
                await Task.Delay(Timeout.Infinite);
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
    }
}
