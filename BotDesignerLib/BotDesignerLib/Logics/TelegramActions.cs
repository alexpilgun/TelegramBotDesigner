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
}
