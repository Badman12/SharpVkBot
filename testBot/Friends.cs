using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model.RequestParams;

namespace testBot
{
    class Friends
    {
        VkApi api;
        public Friends(VkApi api) { this.api = api; }

        public void Run()
        {
            Thread worker = new Thread(new ThreadStart(delegate
            {
                List<long> reqid = new List<long>();
                while (true)
                {
                    var getRequests = api.Friends.GetRequests(new FriendsGetRequestsParams{ });

                    foreach(var req in getRequests)
                    {
                        if (reqid.IndexOf(req.Key) == -1)
                        {
                            reqid.Add(req.Key);
                            api.Messages.Send(new MessagesSendParams
                            {
                                Message = "Привет!! Если вы хотите добавить меня в друзья "
                                + "напишите кто вы, знакомы ли мы, и причину добавления. Именно в таком порядке)))",
                                UserId = req.Key
                                
                            });

                            Thread.Sleep(100);
                            api.Messages.Send(new MessagesSendParams
                            {
                                Message = "Пример: Добавь меня!$<ВАШЕ ИМЯ>$<ОТКУДА ЗНАКОМЫ>$<ПРИЧИНА>",
                                UserId = req.Key
                            });

                            Thread.Sleep(100);
                            api.Messages.Send(new MessagesSendParams
                            {
                                Message = "О и если ты предлагаешь что-то купить, вступить, подписаться и тд, то смело иди на #@%",
                                UserId = req.Key
                            });

                            Thread.Sleep(100);
                            api.Messages.Send(new MessagesSendParams
                            {
                                Message = "для связи с автором используйте ключовое слово автор^^",
                                UserId = req.Key
                            });

                            Thread.Sleep(100);
                            api.Messages.Send(new MessagesSendParams
                            {
                                Message = "Power by VkMegaBot by BaDMaN ^_^",
                                UserId = req.Key
                            });

                        }
                    }
                    Thread.Sleep(500);
                }
            }));

            worker.Start();
        }

        public void AddFriend(long uid)
        {
            api.Friends.Add(uid, "", null, null, null);
        }

    }
}
