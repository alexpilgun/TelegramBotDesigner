using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace ExpenseTrackingBot
{
    public class GoogleSheetsConnectorConfig
    {
        public static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        public static string ApplicationName = "ExpenseTrackingTelegramBot";
        public static FileDataStore DataStore = new FileDataStore("C:\\Users\\sleep\\Documents\\TelegramBotDesignerRepos\\ExpenseTrackingBotCreds\\GoogleUserCredentials", true);
        public static string ProjectCredentialsPath = "C:\\Users\\sleep\\Documents\\TelegramBotDesignerRepos\\ExpenseTrackingBotCreds\\Project_credentials.json";
    }

    public class GoogleSheetsConnector
    {
        public string ChatId { get; }
        public string SpreadsheetId { get; set; }
        public UserCredential UserCredential { get; set; }
        public SheetsService SheetsService { get; set; }

        public GoogleSheetsConnector(long chatId)
        {
            ChatId = chatId.ToString();
        }

        public void AuthorizeUser()
        {
            using (var stream = new FileStream(GoogleSheetsConnectorConfig.ProjectCredentialsPath,
                FileMode.Open, FileAccess.Read))
            {
                UserCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                      GoogleClientSecrets.Load(stream).Secrets,
                      GoogleSheetsConnectorConfig.Scopes,
                      this.ChatId,
                      CancellationToken.None,
                      GoogleSheetsConnectorConfig.DataStore
                      ).Result;
            }
        }

        public void CreateSheetsService()
        {
            SheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = UserCredential,
                ApplicationName = GoogleSheetsConnectorConfig.ApplicationName,
            });
        }
    }
}
