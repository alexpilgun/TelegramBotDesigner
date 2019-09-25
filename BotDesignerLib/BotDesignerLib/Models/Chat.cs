using System;
using System.Linq;

namespace BotDesignerLib
{
    public class Chat
    {
        public readonly long chatId;
        
        public ChatState State { get; set; }

        public Chat(long chatIdInput, IDataContext domainDataContextType)
        {
            chatId = chatIdInput;
            State = new ChatState(this, domainDataContextType);
        }
    }

    public class ChatState
    {
        public Chat Chat { get; set; }
        public MessageBlock CurrentMessageBlock { get; set; }
        public Message CurrentMessage { get; set; }
        public IDataContext DataContext { get; set; }
        public bool WaitForUserTransition { get; set; }
        public bool HasBeenAtLastMessage { get; set; }
        public bool ProcessedUserInput { get; set; }

        public ChatState(Chat newChat, IDataContext domainDataContextType)
        {
            Chat = newChat;
            DataContext = domainDataContextType;
            WaitForUserTransition = false;
        }
    }

    public interface IDataContext
    {

    }
}
