using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotDesignerLib
{
    public class Schema
    {
        public string Name { get; set; }
        public virtual List<SchemaStep> Steps {get;set;}
        public List<SchemaActionBlock> Blocks { get; set; }
        public Schema() { }
    }

    public class SchemaStep
    {
        // ToDo: merge SchemaStep and Transition
        public SchemaActionBlock FromBlock { get; set; }
        public SchemaActionBlock ToBlock { get; set; }
        public Transition Transition { get; set; }

        public SchemaStep()
        {

        }
    }

    public class SchemaActionBlock
    {
        public string Name { get; set; }
        public List<SchemaAction> Messages { get; set; }

        public SchemaActionBlock() { }
    }
 
    public class SchemaAction
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public Func<Chat, string> TextWithProperties { get; set; }
        public Action<Chat, string> PropertySetter { get; set; }
        public Func<string, Chat, TelegramBotClient, LibActionResult> CustomMethod {get;set; }
        public MessageType Type { get; set; }
        public string ErrorHandlingMessage { get; set; }

        public SchemaAction() { }
    }

    [Serializable]
    public class Transition
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsDefault { get; set; }

        public Transition()
        {
            IsDefault = false;
        }

    }

    [Serializable]
    public enum MessageType
    {
        sendMessage,
        sendImage,
        pause,
        saveUserInput,
        Custom

    }
}
