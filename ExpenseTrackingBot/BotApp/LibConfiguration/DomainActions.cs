using System;
using System.Collections.Generic;
using System.Text;
using BotDesignerLib;

namespace ExpenseTrackingBot
{
    public class DomainActions
    {
        public static LibActionResult SaveUserName(string userInput, IDataContext chatDataContext)
        {
            // chatDataContext.UserName = userInput;
            return new LibActionResult() { Status = true };
        }
    }
}
