using System;
using System.Collections.Generic;
using System.Text;

namespace BotDesignerLib
{
    public class ChatStats
    {
        public static string getReportOnCurrentChats(DbConnector dbConnector)
        {
            var result = new StringBuilder();

            var currentChats = dbConnector.chats;
            foreach(var c in currentChats)
            {
                result.Append("ChatId " + c.chatId + " has state " + c.State.CurrentMessageBlock.Name + " & " + c.State.CurrentMessage.Content);
            }
            return result.ToString();
        }
    }
}
