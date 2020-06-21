using Discord.Commands;
using System;
using System.Threading.Tasks;
using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;
using Serilog;
using System.Net;
using System.IO;

namespace DiscordBot
{
    // Keep in mind your module **must** be public and inherit ModuleBase.
    // If it isn't, it will not be discovered by AddModulesAsync!
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
        => ReplyAsync(echo);

        [Command("위키")]
        [Summary("워프레임 위키에서 키워드 검색")]
        public async Task SearchWiki(
        [Summary("검색어")]
        string keyword)
        {
            // We can also access the channel from the Command Context.            
            keyword = Localize.Translate(keyword);
            string[] keyword2 = keyword.Split(" ");
            keyword = "";
            for(int i=0;i<keyword2.Length;i++)
            {
                keyword += keyword2[i];
                if (i != keyword2.Length - 1)
                    keyword += "+";
            }
            //CrawlResult crawlResult = await crawler.CrawlAsync(new Uri("https://pastebin.com/xR9j7MB8"));
            await ReplyAsync("https://warframe.fandom.com/wiki/Special:Search?query=" + keyword);
        }       

        [Command("시터스")]
        [Summary("시터스 시간 검색")]
        public async Task CetusTimer()
        {
            WebRequest request = WebRequest.Create("https://api.warframestat.us/pc/cetusCycle");
            WebResponse response = request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                string[] temp = responseFromServer.Split(',');
                string leftMinute = temp[temp.Length - 1].Split(':')[1].Replace("\"","").Replace("}","");
                if (leftMinute.Contains("Day"))
                    leftMinute = leftMinute.Replace("to Day", "후 낮");
                else
                    leftMinute = leftMinute.Replace("to Night", "후 밤");
                if (leftMinute.Contains("s"))
                    leftMinute = leftMinute.Replace("s", "초");
                if (leftMinute.Contains("m"))
                    leftMinute = leftMinute.Replace("m", "분");
                if (leftMinute.Contains("h"))
                    leftMinute = leftMinute.Replace("h", "시간");
                await ReplyAsync(leftMinute);
            }
        }

        private void SearchCompleted(object sender, PageCrawlCompletedArgs e)
        {
            Console.WriteLine(e.CrawledPage.Content.Text);
            //PrintMessage(e.CrawledPage.Content.Text).GetAwaiter().GetResult();
        }

        public async Task PrintMessage(string Message)
        {
            await ReplyAsync(Message);
        }
    }
}
