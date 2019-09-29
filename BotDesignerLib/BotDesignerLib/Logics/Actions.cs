using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace BotDesignerLib
{
    public static class TelegramActions
    {
        //Todo: add exception handling
        public static bool sendMessage(long chatId, string textToSend, Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup keyboard, TelegramBotClient botClient)
        {

            try
            {
                var messageSent = botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: textToSend,
                    replyMarkup: keyboard
                    );

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool sendImage(long chatId, string imageUrl, Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup keyboard, TelegramBotClient botClient)
        {
            try
            {
                var messageSent = botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: imageUrl,
                    replyMarkup: keyboard
                    );
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public static class LibActions
    {
        public static bool SetDataContextStringProperty(string UserInput, Chat CurrentChat, Action<string, Chat> propertySetter)
        {
            if(!String.IsNullOrEmpty(UserInput))
            {
                propertySetter(UserInput, CurrentChat);
            }

            return true;
        }

        public static bool SetDataContextGenericProperty(string UserInput, Chat CurrentChat, Action<string> propertySetter, Type ModelType)
        {
            return true;
        }
        // ToDo: 
        // 1) add methods for other types: numbers(int, decimal), date
        // 2) Add handling for incorrect input
    }

    public class LibActionInput
    {
        public string userInput { get; set; }
        public IDataContext chatDataContext { get; set; }
        public LibActionInput() { }
    }

    public class LibActionResult
    {
        public bool Status { get; set; }
        public LibActionResult() { }
    }
}
