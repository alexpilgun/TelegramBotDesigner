using System;
using System.Collections.Generic;
using System.Text;

namespace BotDesignerLib
{
    public class Schema
    {
        public string Id { get; }

        public List<SchemaStep> Steps {get;set;}
        public string defaultMessageBlock;

        public Schema(string defaultBlock)
        {
            Id = Guid.NewGuid().ToString("N");
            defaultMessageBlock = defaultBlock;
        }
    }

    public class SchemaStep
    {
        public string Id { get; }
        public MessageBlock FromBlock { get; set; }
        public MessageBlock ToBlock { get; set; }
        public Transition Transition { get; set; }

        public SchemaStep()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }

    public class MessageBlock
    {
        public string Id { get; }
        public string Name { get; set; }
        public List<Message> Messages { get; set; }

        public MessageBlock()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }

    public class Message
    {
        public string Id { get; }
        public string Content { get; set; }
        public string PropertyName { get; set; }
        public Func<string, IDataContext, LibActionResult> CustomMethod;
        public MessageType Type { get; set; }

        public Message()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }

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

    public enum MessageType
    {
        sendMessage,
        sendImage,
        pause,
        saveUserInput,
        Custom

    }
}
