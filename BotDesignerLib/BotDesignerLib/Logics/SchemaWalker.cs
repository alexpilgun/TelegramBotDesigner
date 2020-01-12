using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotDesignerLib
{
    public static class SchemaWalker
    {
        public static void WalkThroughSchema(long currentChatId, string userInput, LibConfigurationModule config, CallbackQuery callbackQuery)
        {
            using (var ctx = (LibDbContext)Activator.CreateInstance(config.DbContextType))
            {
                Chat chat = GetChatById(currentChatId, config, ctx);
                
                do
                {
                    if (callbackQuery != null)
                    {
                        TelegramActions.removeInlineKeyboard(currentChatId, callbackQuery, config.BotClient);
                        callbackQuery = null;
                    }

                    if (chat.State.ProcessedUserInput)
                    {
                        userInput = null;
                    }

                    LibActionResult processedResult = ProcessChatState(chat, userInput, config);
                    if (!processedResult.Status)
                    {
                        string errMsg = processedResult.ErrorMessage ?? chat.State.CurrentMessage.ErrorHandlingMessage;
                        TelegramActions.sendMessage(chat.СhatId, errMsg, null, config.BotClient);
                        break;
                    }
                    StepForward(chat, userInput, config);

                    if (chat.State.CurrentMessage.Type == MessageType.saveUserInput || chat.State.WaitForUserTransition)
                    {
                        chat.State.ProcessedUserInput = false;
                        break;
                    }
                }
                while (true);

                ctx.SaveChanges();
            }
            
            return;
        }

        public static LibActionResult ProcessChatState(Chat chat, string userInput, LibConfigurationModule config)
        {
            if (chat.State.WaitForUserTransition)
            {
                return new LibActionResult() { Status = true };
            }

            Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup customKeyboard = null;
            if ( chat.State.CurrentMessage == chat.State.CurrentMessageBlock.Messages.Last() )
            {
                var possibleSteps = chat.State.Schema.Steps.Where(x => x.FromBlock == chat.State.CurrentMessageBlock).ToList();
                if(possibleSteps.Count >= 2)
                {
                    chat.State.WaitForUserTransition = true;
                    if(chat.State.CurrentMessage.Type != MessageType.saveUserInput || chat.State.HasBeenAtLastMessage)
                    {
                        customKeyboard = KeyboardManagement.createReplyKeyboard(possibleSteps);
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
                        TelegramActions.sendMessage(chat.СhatId, chat.State.CurrentMessage.TextWithProperties(chat), customKeyboard, config.BotClient);
                    }
                    return new LibActionResult() { Status = true };
                case MessageType.sendImage:
                    TelegramActions.sendImage(chat.СhatId, chat.State.CurrentMessage.Content, customKeyboard, config.BotClient);
                    return new LibActionResult() { Status = true };
                case MessageType.saveUserInput:
                    if (chat.State.CurrentMessage.CustomMethod != null)
                    {
                        var result = chat.State.CurrentMessage.CustomMethod(userInput, chat, config.BotClient);
                        chat.State.ProcessedUserInput = result.Status;
                        return result;
                    }
                    else if (chat.State.CurrentMessage.PropertySetter != null)
                    {
                        LibActions.SetDataContextProperty(chat, userInput, chat.State.CurrentMessage.PropertySetter);
                        chat.State.ProcessedUserInput = true;
                        return new LibActionResult() { Status = true };
                    }
                    else 
                    { 
                        return new LibActionResult() { Status = false, ErrorMessage = "Действие не задано" };
                    }
                case MessageType.pause:
                    TelegramActions.sendMessage(chat.СhatId, "Допустим, что тут будет пауза.", customKeyboard, config.BotClient);
                    return new LibActionResult() { Status = true };
                case MessageType.Custom:
                    if(chat.State.CurrentMessage.CustomMethod != null)
                    {
                        return chat.State.CurrentMessage.CustomMethod(userInput, chat, config.BotClient);
                    }
                    else if (chat.State.CurrentMessage.PropertySetter != null)
                    {
                        LibActions.SetDataContextProperty(chat, userInput, chat.State.CurrentMessage.PropertySetter);
                        return new LibActionResult() { Status = true };
                    }
                    else
                    {
                        TelegramActions.sendMessage(chat.СhatId, "Выполнилась кастомная хрень.", customKeyboard, config.BotClient);
                        return new LibActionResult() { Status = true };
                    }
                default:
                    return new LibActionResult() { Status = false };
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
                chat.State.CurrentMessageId = chat.State.CurrentMessage.Id;
                return true;
            }

            if (chat.State.CurrentMessage == chat.State.CurrentMessageBlock.Messages.Last())
            {
                chat.State.HasBeenAtLastMessage = true;
                var allPossibleTransitions = chat.State.Schema.Steps.Where(x => x.FromBlock == currentMessageBlock).Select(x => x.ToBlock).ToList();

                if (allPossibleTransitions != null && allPossibleTransitions.Count == 1)
                {
                    chat.State.CurrentMessageBlock = allPossibleTransitions.First();
                    chat.State.CurrentMessageBlockName = chat.State.CurrentMessageBlock.Name;
                    chat.State.CurrentMessage = allPossibleTransitions.First().Messages.First();
                    chat.State.CurrentMessageId = chat.State.CurrentMessage.Id;
                    chat.State.ProcessedUserInput = true;
                    return true;
                }
                
                var userDefinedSteps = chat.State.Schema.Steps
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
                        chat.State.CurrentMessageBlockName = chat.State.CurrentMessageBlock.Name;
                        chat.State.CurrentMessage = userDefinedSteps.First().Messages.First();
                        chat.State.CurrentMessageId = chat.State.CurrentMessage.Id;
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

        public static Chat GetChatById(long currentChatId, LibConfigurationModule config, LibDbContext ctx)
        {
            var chat = ctx.Chats.Where(x => x.СhatId == currentChatId).FirstOrDefault();

            if (chat == null)
            {
                chat = new Chat(currentChatId, config);
                ctx.Chats.Add(chat);
                ctx.SaveChanges();
            }

            BindChatToSchemaHelpers.UpdateChatToSchemasReferences(chat, config);
            return chat;
        }
    }
}
