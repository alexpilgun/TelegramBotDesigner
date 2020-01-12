using System;
using System.Collections.Generic;
using System.Text;
using BotDesignerLib;
using Telegram.Bot;
using MihaZupan;
using System.Linq;


namespace ExpenseTrackingBot
{
    public class LibInit
    {
        public string TelegramBotAccessToken { get; set; }
        public LibConfigurationModule Config { get; set; }

        public LibInit()
        {
            TelegramBotAccessToken = System.IO.File.ReadAllText("C:\\Users\\sleep\\Documents\\TelegramBotDesignerRepos\\ExpenseTrackingBotCreds\\TelegramBotAccessToken.txt");

            Config = new LibConfigurationModule()
            {
                BotClient = new TelegramBotClient(TelegramBotAccessToken, new HttpToSocks5Proxy("127.0.0.1", 9054)),
                DbContextType = typeof(AppDbContext),
                DomainDataContextType = typeof(DomainDataContext)
            };

            Config.SchemasRepository.Add("default", new DefaultSchemaInstance());
        }
    }
}
