using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DiscordBot
{
    class Localize
    {
        static Dictionary<string, string> _Dic_Localize;
        public static void Init()
        {
            _Dic_Localize = new Dictionary<string, string>();
            WebRequest request = WebRequest.Create("https://pastebin.com/raw/xR9j7MB8");
            WebResponse response = request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                string[] temp1 = responseFromServer.Split('\n');
                for(int i=0;i<temp1.Length;i++)
                {
                    string[] temp2 = temp1[i].Split(',');
                    _Dic_Localize.Add(temp2[0], temp2[1]);
                }
            }

            response.Close();
        }

        public static string Translate(string Keyword)
        {
            if (_Dic_Localize.ContainsKey(Keyword) == false)
                return string.Empty;
            else
                return _Dic_Localize[Keyword];
        }
    }
}
