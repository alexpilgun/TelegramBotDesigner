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
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        [Key]
        public string ChatId { get; set; }
        public string SpreadsheetId { get; set; }
        public string Sheetname { get; set; }
        public int? SheetId { get; set; }
        [NotMapped]
        public UserCredential UserCredential { get; set; }
        [NotMapped]
        public SheetsService SheetsService { get; set; }
        public virtual DomainDataContext DomainDataContext { get; set; }
        public string DomainDataContextId { get; set; }

        public GoogleSheetsConnector() { }

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
