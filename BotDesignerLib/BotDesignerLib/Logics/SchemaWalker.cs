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
            Chat chat = config.DbConnector.chats.Where(x => x.chatId == currentChatId).FirstOrDefault();

            if (chat == null)
            {
                IDataContext chatDataContext = (IDataContext)Activator.CreateInstance(config.DomainDataContextType);
                chat = new Chat(currentChatId, chatDataContext);
                chat.State.CurrentMessageBlock = config.Schema.Steps
                    .Where(x => x.FromBlock.Name == config.Schema.defaultMessageBlock).Select(x => x.FromBlock).First();
                chat.State.CurrentMessage = chat.State.CurrentMessageBlock.Messages.First();
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
                var possibleSteps = config.Schema.Steps.Where(x => x.FromBlock == chat.State.CurrentMessageBlock).ToList();
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
                    TelegramActions.sendMessage(chat.chatId, chat.State.CurrentMessage.Content, customKeyboard, config.BotClient);
                    return true;
                case MessageType.sendImage:
                    TelegramActions.sendImage(chat.chatId, chat.State.CurrentMessage.Content, customKeyboard, config.BotClient);
                    return true;
                case MessageType.saveUserInput:
                    TelegramActions.sendMessage(chat.chatId, "Допустим, что сохранили:" + userInput, customKeyboard, config.BotClient);
                    chat.State.ProcessedUserInput = true;
                    return true;
                case MessageType.pause:
                    TelegramActions.sendMessage(chat.chatId, "Допустим, что тут будет пауза.", customKeyboard, config.BotClient);
                    return true;
                case MessageType.Custom:
                    // ToDo: call custom method (with reflections?)
                    TelegramActions.sendMessage(chat.chatId, "Выполнилась кастомная хрень.", customKeyboard, config.BotClient);
                    return true;
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
                var allPossibleTransitions = config.Schema.Steps.Where(x => x.FromBlock == currentMessageBlock).Select(x => x.ToBlock).ToList();

                if (allPossibleTransitions != null && allPossibleTransitions.Count == 1)
                {
                    chat.State.CurrentMessageBlock = allPossibleTransitions.First();
                    chat.State.CurrentMessage = allPossibleTransitions.First().Messages.First();
                    chat.State.ProcessedUserInput = true;
                    return true;
                }
                
                var userDefinedSteps = config.Schema.Steps
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
