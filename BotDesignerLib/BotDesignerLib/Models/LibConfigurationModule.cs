using System;
using System.Collections.Generic;
using System.Text;
using BotDesignerLib;
using Telegram.Bot;


namespace BotDesignerLib
{
    public class LibConfigurationModule
    {
        public TelegramBotClient BotClient { get; set; }
        public DbConnector DbConnector {get;set; }
        public Schema Schema {get;set; }
        public Type DomainDataContextType { get; set; }

        public LibConfigurationModule()
        {

        }
    }
}
