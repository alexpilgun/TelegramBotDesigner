using System;
using System.Collections.Generic;
using System.Text;
using BotDesignerLib;
using Telegram.Bot;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace BotDesignerLib
{
    public class LibConfigurationModule
    {
        public TelegramBotClient BotClient { get; set; }
        public Type DomainDataContextType { get; set; }
        public Type DbContextType { get; set; }
        public Dictionary<string, Schema> SchemasRepository { get; set; }

        public LibConfigurationModule()
        {
            SchemasRepository = new Dictionary<string, Schema>();
        }
    }
}
