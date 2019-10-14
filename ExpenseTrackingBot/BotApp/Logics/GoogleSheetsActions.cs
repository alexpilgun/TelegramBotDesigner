using System;
using System.Collections.Generic;
using System.Text;

using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

using BotDesignerLib;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExpenseTrackingBot
{
    public static class GoogleSheetsActions
    {
        public static LibActionResult InsertExpenseToGoogleSheet(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var connector = ((DomainDataContext)chat.DataContext).GoogleSheetsConnector;


            // get first free row
            // form newCellsValues from Expense object
            // check status and add error handling (repeated request?)

            /*
            IList<IList<object>> newCellsValues = new List<IList<object>>() { new List<object>() { "John Snow", "Male" } };
            ValueRange vr = new ValueRange();
            vr.Values = newCellsValues;
            */
            /*
            var updateRequest = connector.SheetsService.Spreadsheets.Values.Append(vr, connector.SpreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
            var result = updateRequest.Execute();
            */

            // Console.WriteLine(result.Updates.UpdatedCells);

            return new LibActionResult() { Status = true };
        }
    }
}
