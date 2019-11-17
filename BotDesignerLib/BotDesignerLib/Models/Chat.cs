using System;
using System.Linq;

namespace BotDesignerLib
{
    [Serializable]
    public class Chat
    {
        public readonly long СhatId;
        public Schema Schema { get; set; }
        public State State { get; set; }
        public IDataContext DataContext { get; set; }

        public Chat(long chatIdInput, LibConfigurationModule config)
        {
            СhatId = chatIdInput;
            DataContext = (IDataContext)Activator.CreateInstance(config.DomainDataContextType, chatIdInput);
            Schema = (Schema)Activator.CreateInstance(config.DomainSchemaType);
            Schema.Chat = this;
            State = new State(this);
        }
    }
    [Serializable]
    public class State
    {
        //public Chat Chat { get; set; }
        public SchemaActionBlock CurrentMessageBlock { get; set; }
        public SchemaAction CurrentMessage { get; set; }
        public bool WaitForUserTransition { get; set; }
        public bool HasBeenAtLastMessage { get; set; }
        public bool ProcessedUserInput { get; set; }

        public State(Chat chat)
        {
            //Chat = chat;
            CurrentMessageBlock = chat.Schema.Steps.First().FromBlock;
            CurrentMessage = this.CurrentMessageBlock.Messages.First();
            WaitForUserTransition = false;
        }
    }
}
