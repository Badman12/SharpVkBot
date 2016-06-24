using System;
using System.Text.RegularExpressions;
using System.Threading;
using VkNet;
using VkNet.Model.RequestParams;
using System.Speech.Synthesis;
using ConsoleBot;


namespace testBot
{
    class Messeges
    {
        VkApi api;
        public Messeges(VkApi api) { this.api = api; }
        
        public void Run()
        {
            Thread worker = new Thread(new ThreadStart(delegate
            {
                Console.WriteLine("Принимаем сообщения...");

                BaDMan bot = new BaDMan();
                bot.Initialize();
                while (true)
                {
                    var get = api.Messages.Get(new MessagesGetParams() { Count = 100 });

                    foreach (var mes in get.Messages)
                    {
                        try {

                            //овечаем на непрочитаные сообщения 
                            if (mes.ReadState == VkNet.Enums.MessageReadState.Unreaded)
                            {
                                
                                if (commands("add", mes.Body) )// добавляет в френды 
                                {
                                    new Friends(api).AddFriend((long)mes.UserId);
                                    Console.WriteLine("ДОбавили друга");
                                }
                                else if (commands("ban", mes.Body))// банит всех кто сказал каку 
                                {
                                    api.Messages.Send(new MessagesSendParams { Message = "Spam deff :D You Baned ",
                                        UserId = mes.UserId });
                                    api.Account.BanUser((long)mes.UserId);
                                }
                                else if (commands("author",mes.Body))// удалите крч безполезная фигня
                                {
                                    api.Messages.Send(new MessagesSendParams
                                    {
                                        Message = mes.Body + " From http://vk.com/id" + mes.UserId,
                                        UserId = 65148930 // типа когда ключевое слово автор то сообщение идет мне
                                    });
                                    api.Messages.Delete((ulong)mes.Id);
                                }
                                else {

                                    speech(mes.Body, (long)mes.UserId, (long)mes.Id);
                                    //Если хотите чтобы чисто озвучивало сообшения то отмечайте их как прочтенные
                                    //api.Messages.MarkAsRead((long)mes.Id); 

                                    Console.WriteLine();
                                        Console.Write("СООБЩЕНИЕ: ");
                                        Console.WriteLine(mes.Body + "\n");
                                        Console.WriteLine("ОТВЕТ:");
                                        string reply = bot.getOutput(mes.Body);
                                        Console.Write(reply);
                                        api.Messages.Send(new MessagesSendParams { Message = reply, ChatId = mes.ChatId, UserId = mes.UserId });
                                    
                                    Thread.Sleep(500);
                                }
                            }
                        }
                        catch { }
                    }

                    Thread.Sleep(1000);
                }
            }));

            worker.Start();
        }

        public bool commands(string comm,string mess)
        {
            string pattern;
            switch (comm)
            {
                case "add":
                    pattern = "Добавь меня!|Го дружить|Добавь в друзья";
                    break;
                case "ban":
                    pattern = "курсові|дипломні|реферати|курсовые|дипломные|рефераты|подпишись|приєднуйся до|підпишись";
                    break;
                case "author":
                    pattern = "автор|Автор|Создатель|Создатель|Разработчик|разработчик";
                    break;
                default:
                    Console.WriteLine("CommandError ;(");
                    return false;
                    break;
            }
            Regex regex = new Regex(pattern);
            Match match = regex.Match(mess);
            if (match.Value != String.Empty)
                return true;
            else
                return false;
        } 

        public void speech(string mess, long userid, long mesid)
        {
            SpeechSynthesizer speaker = new SpeechSynthesizer();
            var voiceList = speaker.GetInstalledVoices();
            speaker.SelectVoice(voiceList[0].VoiceInfo.Name);
            speaker.SetOutputToDefaultAudioDevice();
            speaker.Rate = 0;
            speaker.Volume = 100;
            var p = api.Users.Get(userid, VkNet.Enums.Filters.ProfileFields.Sex);
            string speechText= "Cообщение от ";
            if(p.Sex == VkNet.Enums.Sex.Female)
            {
                speechText += " шкуры ";
            }
            else if (p.Sex == VkNet.Enums.Sex.Male)
            {
                if (p.Id == 85151069)
                    speechText += " Бога ";
                else if (p.Id == 65148930)
                    speechText += " самого крутого и охуенного чувака на всей обосраной планете, короля мира и всея интернета ";
                else
                    speechText += " уебана ";
            }
            else
            {
                speechText += " пидара ";
            }
            speaker.SpeakAsync(speechText + p.FirstName + " " + p.LastName);
            speaker.SpeakAsync(mess);
            api.Messages.MarkAsRead(mesid);
        }

    }
}
