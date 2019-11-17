using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using LiteDB;

namespace BotDesignerLib
{
    [Serializable]
    public class Schema
    {
        public string Id { get; }
        [BsonIgnore]
        public Chat Chat;
        public List<SchemaStep> Steps {get;set;}

        public Schema()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }
    [Serializable]
    public class SchemaStep
    {
        // ToDo: merge SchemaStep and Transition
        public string Id { get; }
        public SchemaActionBlock FromBlock { get; set; }
        public SchemaActionBlock ToBlock { get; set; }
        public Transition Transition { get; set; }

        public SchemaStep()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }
    [Serializable]
    public class SchemaActionBlock
    {
        public string Id { get; }
        public string Name { get; set; }
        public List<SchemaAction> Messages { get; set; }

        public SchemaActionBlock()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }
    [Serializable]
    public class SchemaAction
    {
        public string Id { get; }
        public string Content { get; set; }
        public Func<string, string> TextWithProperties { get; set; }
        public Action<string> PropertySetter { get; set; }
        public Func<string, Chat, TelegramBotClient, LibActionResult> CustomMethod {get;set; }
        public MessageType Type { get; set; }
        public string ErrorHandlingMessage { get; set; }

        public SchemaAction()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }

    [Serializable]
    public class Transition
    {
        public string Id { get; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsDefault { get; set; }

        public Transition()
        {
            Id = Guid.NewGuid().ToString("N");
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
