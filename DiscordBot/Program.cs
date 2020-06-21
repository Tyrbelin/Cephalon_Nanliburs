using System;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

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
            _client = new DiscordSocketClient();

            _client.Log += Log;

            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
            //var token = Environment.GetEnvironmentVariable("BOT_TOKEN",EnvironmentVariableTarget.Machine);
            var token = Environment.GetEnvironmentVariable("BOT_TOKEN");
            //await Ping();
            // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
            // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
            // var token = File.ReadAllText("token.txt");
            // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            _handler = new CommandHandler(_client,new Discord.Commands.CommandService());
            await _handler.InstallCommandsAsync();
            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task Ping()
        {
            while(true)
            {
                WebRequest request = WebRequest.Create("https://cephalon-nanliburs.herokuapp.com/");
                WebResponse response = request.GetResponse();
                await Task.Delay(30000);
            }
        }
    }
}
