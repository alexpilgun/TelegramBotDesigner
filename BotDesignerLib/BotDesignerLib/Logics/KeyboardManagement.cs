using System;
using System.Collections.Generic;
using System.Text;

namespace BotDesignerLib
{
    public static class KeyboardManagement
    {
        public static Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup createReplyKeyboard(List<SchemaStep> possibleSteps)
        {
            var buttonsRow = new List<Telegram.Bot.Types.ReplyMarkups.KeyboardButton>();

            foreach (var t in possibleSteps)
            {
                buttonsRow.Add(new Telegram.Bot.Types.ReplyMarkups.KeyboardButton(t.Transition.DisplayName));
            }

            var replyKeyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup()
            {
                OneTimeKeyboard = true,
                ResizeKeyboard = true
            };

            replyKeyboard.Keyboard = new List<List<Telegram.Bot.Types.ReplyMarkups.KeyboardButton>>()
            {
                buttonsRow
            };
            return replyKeyboard;
        }

        public static Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup createInlineKeyboard(List<string> buttonNames)
        {
            var buttonsRow = new List<Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton>();

            foreach (var b in buttonNames)
            {
                var button = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton();
                button.Text = b;
                buttonsRow.Add(button);
            }

            var inlineKeyboard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(buttonsRow);
            return inlineKeyboard;
        }
    }
}
