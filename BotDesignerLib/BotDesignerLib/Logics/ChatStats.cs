using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BotDesignerLib
{
    public class ChatStats
    {
        public static string getReportOnCurrentChats(LibConfigurationModule config)
        {
            var result = new StringBuilder();
            result.Append($"{DateTime.Now.ToString()} {System.Environment.NewLine}");

            using (var ctx = (LibDbContext)Activator.CreateInstance(config.DbContextType))
            {
                var chats = ctx.Chats.ToList();
                foreach (var c in chats)
                {
                    result.Append("ChatId " + c.СhatId.ToString() + " has state " + c.State.CurrentMessageBlockName);
                }
            }
            result.Append($"{System.Environment.NewLine}");
            return result.ToString();
        }
    }
}
