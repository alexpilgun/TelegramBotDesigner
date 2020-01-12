using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
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
            var expense = ((DomainDataContext)chat.DataContext).CurrentExpense;
            bool sheetnameHasChanged = false;
            //int googleSheetsDate = Convert.ToInt32( (expense.ExpenseDate.Date - new DateTime(1899, 12, 30) ).TotalDays);

            IList<IList<object>> newCellsValues = new List<IList<object>>() { new List<object>() 
            { expense.ExpenseValue, expense.Category.Name, expense.ExpenseDate.Date.ToString("yyy-MM-dd")} };

            var r = GoogleAuthActions.AuthorizeAndGetSheetsService(userInput, chat, botClient);
            if (r.Status == false)
            {
                return new LibActionResult()
                {
                    Status = false,
                    ErrorMessage = "Не удалось авторизовать твой аккаунт у Гугла:("
                };
            }

            ValueRange vr = new ValueRange();
            vr.Values = newCellsValues;
            var updateRequest = connector.SheetsService.Spreadsheets.Values.Append(vr, connector.SpreadsheetId, connector.Sheetname);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            try
            {
                var result = updateRequest.Execute();
            }
            catch (Google.GoogleApiException e)
            {
                if (e.Error.Code == 400)
                {
                    sheetnameHasChanged = true;
                }

                else
                {
                    return new LibActionResult()
                    {
                        Status = false,
                        ErrorMessage = "Unknow error from Google, try again"
                    };
                }
            }

            if(sheetnameHasChanged)
            {
                try
                {
                    connector.Sheetname = connector.SheetsService.Spreadsheets.Get(connector.SpreadsheetId)
                        .Execute().Sheets.Where(x => x.Properties.SheetId == connector.SheetId).First().Properties.Title;

                    updateRequest = connector.SheetsService.Spreadsheets.Values.Append(vr, connector.SpreadsheetId, connector.Sheetname);
                    updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;

                }
                catch (Google.GoogleApiException anotherE)
                {
                    return new LibActionResult()
                    {
                        Status = false,
                        ErrorMessage = "Изменен лист в GoogleSheets"
                    };
                }
            }

            return new LibActionResult() { Status = true };
        }
    }
}
