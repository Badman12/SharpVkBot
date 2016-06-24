using AIMLbot;
using System;
using System.Net;
using System.Text;

namespace ConsoleBot
{
    public class BaDMan
    {
        private Bot myBot;
        private User myUser;

        public object ConfigurationManager { get; private set; }

        /// <summary>
        /// Create a new instance of the ALICE object
        /// </summary>
        public BaDMan()
        {
            myBot = new Bot();
            myUser = new User("consoleUser", myBot);
        }

        /// <summary>
        /// This initialization can be put in the alice() method
        /// but I kept it seperate due to the nature of my program.
        /// This method loads all the AIML files located in the \AIML folder
        /// </summary>
        public void Initialize()
        {
            myBot.loadSettings();
            myBot.isAcceptingUserInput = false;
            myBot.loadAIMLFromFiles();
            myBot.isAcceptingUserInput = true;
        }

        /// <summary>
        /// This method takes an input string, then finds a response using the the AIMLbot library and returns it
        /// </summary>
        /// <param name="input">Input Text</param>
        /// <returns>Response</returns>
        public String getOutput(String input)
        {
            Request r = new Request(Translate(input, "ru", "en"), myUser, myBot);
            Result res = myBot.Chat(r);
            return (Translate(res.Output, "en" , "ru"));
        }

        private string Translate(string text, string from, string to)
        {
            string page = null;
            try
            {
                WebClient wc = new WebClient();
                wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
                wc.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                wc.Encoding = Encoding.UTF8;

                string url = string.Format(@"http://translate.google.com.tr/m?hl=en&sl={0}&tl={1}&ie=UTF-8&prev=_m&q={2}",
                                            from, to, Uri.EscapeUriString(text));

                page = wc.DownloadString(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

            page = page.Remove(0, page.IndexOf("<div dir=\"ltr\" class=\"t0\">")).Replace("<div dir=\"ltr\" class=\"t0\">", "");
            int last = page.IndexOf("</div>");
            page = page.Remove(last, page.Length - last);

            return page;
        }

    }
}
