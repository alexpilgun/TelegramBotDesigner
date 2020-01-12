using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BotDesignerLib
{
    public class Chat
    {
        [Key]
        public long СhatId { get; set; }
        [Required]
        public virtual State State { get; set; }
        [Required]
        public virtual DataContext DataContext { get; set; }
        public Chat() { }

        public Chat(long chatIdInput, LibConfigurationModule config)
        {
            СhatId = chatIdInput;
            DataContext = (DataContext)Activator.CreateInstance(config.DomainDataContextType, chatIdInput);
            State = new State(this, config);
        }
    }

    public class State
    {
        [Key]
        public long Id { get; set; }
        [NotMapped]
        public Schema Schema { get; set; }
        [Required]
        public string SchemaName { get; set; }
        [NotMapped]
        public SchemaActionBlock CurrentMessageBlock { get; set; }
        [Required]
        public string CurrentMessageBlockName { get; set; }
        [NotMapped]
        public SchemaAction CurrentMessage { get; set; }
        [Required]
        public string CurrentMessageId { get; set; }
        public bool WaitForUserTransition { get; set; }
        public bool HasBeenAtLastMessage { get; set; }
        public bool ProcessedUserInput { get; set; }
        public State() { }
        public State(Chat chat, LibConfigurationModule config)
        {
            Id = chat.СhatId;
            //todo: wrap into method with exception thrown similiar to BindChatToSchemaHelpers
            Schema = config.SchemasRepository["default"];
            SchemaName = "default";
            CurrentMessageBlock = Schema.Steps.First().FromBlock;
            CurrentMessageBlockName = CurrentMessageBlock.Name;
            CurrentMessage = CurrentMessageBlock.Messages.First();
            CurrentMessageId = CurrentMessage.Id;
            WaitForUserTransition = false;
        }
    }
}
