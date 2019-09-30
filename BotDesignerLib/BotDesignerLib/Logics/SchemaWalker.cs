using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Telegram.Bot;

namespace BotDesignerLib
{
    public static class SchemaWalker
    {
        public static void WalkThroughSchema(long currentChatId, string userInput, LibConfigurationModule config)
        {
            Chat chat = config.DbConnector.chats.Where(x => x.СhatId == currentChatId).FirstOrDefault();
            var v = 5;
            if (chat == null)
            {
                chat = new Chat(currentChatId, config);
                config.DbConnector.chats.Add(chat);
            }

            bool wasProcessed;

            do
            {
                if(chat.State.ProcessedUserInput)
                {
                    userInput = null;
                }

                wasProcessed = ProcessChatState(chat, userInput, config);
                if(!wasProcessed)
                {
                    break;
                }
                StepForward(chat, userInput, config);

                if(chat.State.CurrentMessage.Type == MessageType.saveUserInput || chat.State.WaitForUserTransition)
                {
                    chat.State.ProcessedUserInput = false;
                    break;
                }
            }
            while (true) ;

            return;
        }

        public static bool ProcessChatState(Chat chat, string userInput, LibConfigurationModule config)
        {
            if (chat.State.WaitForUserTransition)
            {
                return true;
            }

            Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup customKeyboard = null;
            if ( chat.State.CurrentMessage == chat.State.CurrentMessageBlock.Messages.Last() )
            {
                var possibleSteps = chat.Schema.Steps.Where(x => x.FromBlock == chat.State.CurrentMessageBlock).ToList();
                if(possibleSteps.Count >= 2)
                {
                    chat.State.WaitForUserTransition = true;
                    if(chat.State.CurrentMessage.Type != MessageType.saveUserInput || chat.State.HasBeenAtLastMessage)
                    {
                        customKeyboard = createCustomKeyboard(possibleSteps);
                    }
                }
            }
            
            switch(chat.State.CurrentMessage.Type)
            {
                case MessageType.sendMessage:
                    if(!String.IsNullOrEmpty(chat.State.CurrentMessage.Content))
                    {
                        TelegramActions.sendMessage(chat.СhatId, chat.State.CurrentMessage.Content, customKeyboard, config.BotClient);
                    }
                    else if(chat.State.CurrentMessage.TextWithProperties != null)
                    {
                        TelegramActions.sendMessage(chat.СhatId, chat.State.CurrentMessage.TextWithProperties(userInput), customKeyboard, config.BotClient);
                    }
                    return true;
                case MessageType.sendImage:
                    TelegramActions.sendImage(chat.СhatId, chat.State.CurrentMessage.Content, customKeyboard, config.BotClient);
                    return true;
                case MessageType.saveUserInput:
                    if (chat.State.CurrentMessage.CustomMethod != null)
                    {
                        chat.State.CurrentMessage.CustomMethod(userInput);
                    }
                    else if(chat.State.CurrentMessage.PropertySetter != null)
                    {
                        LibActions.SetDataContextStringProperty(userInput, chat.State.CurrentMessage.PropertySetter);
                    }

                    //TelegramActions.sendMessage(chat.chatId, "Допустим, что сохранили:" + userInput, customKeyboard, config.BotClient);
                    chat.State.ProcessedUserInput = true;
                    return true;
                case MessageType.pause:
                    TelegramActions.sendMessage(chat.СhatId, "Допустим, что тут будет пауза.", customKeyboard, config.BotClient);
                    return true;
                case MessageType.Custom:
                    if(chat.State.CurrentMessage.CustomMethod != null)
                    {
                        chat.State.CurrentMessage.CustomMethod(userInput);
                        return true;
                    }
                    else
                    {
                        TelegramActions.sendMessage(chat.СhatId, "Выполнилась кастомная хрень.", customKeyboard, config.BotClient);
                        return true;
                    }
                default:
                    return false;
            }
        }

        public static bool StepForward(Chat chat, string transitionInput, LibConfigurationModule config)
        {
            var currentMessageBlock = chat.State.CurrentMessageBlock;
            var currentMessage = chat.State.CurrentMessage;

            if (chat.State.CurrentMessage != chat.State.CurrentMessageBlock.Messages.Last())
            {
                var currentIndex = chat.State.CurrentMessageBlock.Messages.FindIndex(x => x == currentMessage);
                chat.State.CurrentMessage = chat.State.CurrentMessageBlock.Messages[currentIndex + 1];
                return true;
            }

            

            if (chat.State.CurrentMessage == chat.State.CurrentMessageBlock.Messages.Last())
            {
                chat.State.HasBeenAtLastMessage = true;
                var allPossibleTransitions = chat.Schema.Steps.Where(x => x.FromBlock == currentMessageBlock).Select(x => x.ToBlock).ToList();

                if (allPossibleTransitions != null && allPossibleTransitions.Count == 1)
                {
                    chat.State.CurrentMessageBlock = allPossibleTransitions.First();
                    chat.State.CurrentMessage = allPossibleTransitions.First().Messages.First();
                    chat.State.ProcessedUserInput = true;
                    return true;
                }
                
                var userDefinedSteps = chat.Schema.Steps
                    .Where(x => x.FromBlock == currentMessageBlock && x.Transition.DisplayName == transitionInput)
                    .Select(x => x.ToBlock).ToList();

                if (userDefinedSteps == null)
                {
                    return false;
                }

                switch(userDefinedSteps.Count)
                {
                    case 0:
                        return true;
                    case 1:
                        chat.State.CurrentMessageBlock = userDefinedSteps.First();
                        chat.State.CurrentMessage = userDefinedSteps.First().Messages.First();
                        chat.State.WaitForUserTransition = false;
                        chat.State.HasBeenAtLastMessage = false;
                        chat.State.ProcessedUserInput = true;
                        return true;
                    default:
                        throw new ApplicationException("2 or more user-defined transitions exist.");

                }
            }

            return false;
        }

        public static Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup createCustomKeyboard(List<SchemaStep> possibleSteps)
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
    }
}
