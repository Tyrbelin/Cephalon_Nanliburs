using Discord.Commands;
using System;
using System.Threading.Tasks;
using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;
using Serilog;

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

        [Command("검색", RunMode = RunMode.Async)]
        [Summary("워프레임 위키에서 키워드 검색")]
        public async Task SearchWiki(
        [Summary("검색어")]
        string keyword)
        {
            // We can also access the channel from the Command Context.
            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = 10,
                MinCrawlDelayPerDomainMilliSeconds = 3000
            };
            PoliteWebCrawler crawler = new PoliteWebCrawler(config);
            crawler.PageCrawlCompleted += SearchCompleted;
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
