using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;

namespace testBot
{
    class Program
    {
        static void Main(string[] args)
        {
            int appID = 5408593;                      //app id
            var vk = new VkApi();
            while (true)
            {
                Console.Write("LOG: ");
                string email = Console.ReadLine();    // email or phone
                Console.Write("PWD: ");
                string pass = Console.ReadLine(); // password
                Settings scope = Settings.All;

                ApiAuthParams my = new ApiAuthParams
                {
                    ApplicationId = (ulong)appID,
                    Login = email,
                    Password = pass,
                    Settings = scope
                };

                //авторизация
                try
                {
                    vk.Authorize(my);
                    Console.Clear();
                    break;
                }
                catch(Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                }
            }

        
        /* Наверное нельзя юзать все 2 сразу 
        *  Ибо ошибка в апи будет со временем
        *  Там мона сделать чтобы не было
        *  Но мне и так норм */


        new Messeges(vk).Run();

        new Friends(vk).Run();

        

        Console.ReadKey();

        }
    }
}
