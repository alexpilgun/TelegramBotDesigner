using System;
using System.Collections.Generic;
using System.Text;

namespace BotDesignerLib
{
    public class ChatStats
    {
        public static string getReportOnCurrentChats(IDbConnector dbConnector)
        {
            var result = new StringBuilder();

            var currentChats = dbConnector.GetAllChats();
            foreach(var c in currentChats)
            {
                result.Append("ChatId " + c.СhatId + " has state " + c.State.CurrentMessageBlock.Name + " & " + c.State.CurrentMessage.Content);
            }
            return result.ToString();
        }
    }
}
