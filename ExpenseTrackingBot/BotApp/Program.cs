using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using System.Threading;
using System.Net;
using MihaZupan;
using System.Linq;
using System.Collections.Generic;
using BotDesignerLib;

namespace ExpenseTrackingBot
{
    class Program
    {
        public static LibConfigurationModule config { get; set; }

        static void Main(string[] args)
        {
            config = new LibConfigurationModule()
            {
                DbConnector = new DbConnector(),
                Schema = new SchemaInstance().schema,
                BotClient = new TelegramBotClient("713947361:AAF9PylFSAAd3Bi1xKseyEaoPukwIw1FJwk", new HttpToSocks5Proxy("127.0.0.1", 9054)),
                DomainDataContextType = typeof(DomainDataContext)
            };
        
            var initializationStatus = config.BotClient.TestApiAsync().Result;

            config.BotClient.OnMessage += Bot_OnMessage;
            config.BotClient.StartReceiving();

            while (true)
            {
                Console.WriteLine(ChatStats.getReportOnCurrentChats(config.DbConnector));
                Thread.Sleep(10000);
            }
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            SchemaWalker.WalkThroughSchema(e.Message.Chat.Id, e.Message.Text, config);
        }
    }
}
