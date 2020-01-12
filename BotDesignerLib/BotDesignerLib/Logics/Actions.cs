using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotDesignerLib
{
    public static class TelegramActions
    {
        //ToDo: add exception handling
        public static bool sendMessage(long chatId, string textToSend, IReplyMarkup keyboard, TelegramBotClient botClient)
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

        public static bool editMessage(long chatId, int messageId, string textToEdit, InlineKeyboardMarkup keyboard, TelegramBotClient botClient)
        {
            try
            {
                //var editMessage = botClient.EditMessageTextAsync(ChatId chat, );
                //(ChatId chatId, int messageId, string textToEdit);
                botClient.EditMessageTextAsync(
                            chatId,
                            messageId,
                            textToEdit,
                            replyMarkup: keyboard
                        );

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool removeInlineKeyboard(long chatId, CallbackQuery callbackQuery, TelegramBotClient botClient)
        {
            var result = editMessage(chatId, callbackQuery.Message.MessageId, callbackQuery.Message.Text, null, botClient);

            return true;
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
        public static bool SetDataContextProperty(Chat CurrentChat, string UserInput, Action<Chat, string> propertySetter)
        {
            if(!String.IsNullOrEmpty(UserInput))
            {
                
            }
            propertySetter(CurrentChat, UserInput);
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
        public DataContext chatDataContext { get; set; }
        public LibActionInput() { }
    }

    public class LibActionResult
    {
        public bool Status { get; set; }
        public string ErrorMessage { get; set; }
        public LibActionResult() { }
    }
}
