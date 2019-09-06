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
        public static TelegramBotClient botClient;
        public static DbConnector dbConnector;
        public static Schema schema;

        static void Main(string[] args)
        {
            dbConnector = new DbConnector();
            SchemaInstance currentSchemaInstance = new SchemaInstance();
            schema = currentSchemaInstance.schema;

            botClient = new TelegramBotClient(
                "713947361:AAF9PylFSAAd3Bi1xKseyEaoPukwIw1FJwk", new HttpToSocks5Proxy("127.0.0.1", 9054));
    
            var initializationStatus = botClient.TestApiAsync().Result;

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

            while (true)
            {
                Console.WriteLine(ChatStats.getReportOnCurrentChats(dbConnector));
                Thread.Sleep(10000);
            }
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            SchemaWalker.WalkThroughSchema(e.Message.Chat.Id, e.Message.Text, botClient, schema, dbConnector);

            //await botClient.SendTextMessageAsync(
            //    chatId: e.Message.Chat.Id,
            //    text: "testing buttons",
            //    replyMarkup: createCustomKeyboard()
            //    );
        }
    }
}
