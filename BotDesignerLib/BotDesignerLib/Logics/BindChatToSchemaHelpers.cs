using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BotDesignerLib
{
    public static class BindChatToSchemaHelpers
    {
        public static void UpdateChatToSchemasReferences(Chat c, LibConfigurationModule config)
        {
            BindChatToSchemaHelpers.BindSchema(c, config);
            BindChatToSchemaHelpers.BindMessageBlock(c, config);
            BindChatToSchemaHelpers.BindMessage(c, config);
        }

        public static void BindSchema(Chat c, LibConfigurationModule config)
        {
            Schema chatSchema = null;
            config.SchemasRepository.TryGetValue(c.State.SchemaName, out chatSchema);
            if (chatSchema == null)
            {
                throw new Exception($"Schema \"{c.State.SchemaName}\" is not found for chat {c.СhatId.ToString()}");
            }
            else
            {
                c.State.Schema = chatSchema;
            }
            return;
        }

        public static void BindMessageBlock(Chat c, LibConfigurationModule config)
        {
            var messageBlocksCandidates = c.State.Schema.Blocks.Where(x => x.Name == c.State.CurrentMessageBlockName);
            if (messageBlocksCandidates == null)
            {
                throw new Exception($"Message block \"{c.State.CurrentMessageBlockName}\" is not found in schema \"{c.State.SchemaName}\" for chat {c.СhatId.ToString()}");
            }
            else if (messageBlocksCandidates.Count() != 1)
            {
                throw new Exception($"There are several message blocks \"{c.State.CurrentMessageBlockName}\" in schema \"{c.State.SchemaName}\" for chat {c.СhatId.ToString()}");
            }
            else
            {
                c.State.CurrentMessageBlock = messageBlocksCandidates.First();
            }
            return;
        }

        public static void BindMessage(Chat c, LibConfigurationModule config)
        {
            var messageCandidates = c.State.Schema.Blocks.Where(x => x.Name == c.State.CurrentMessageBlockName)
                .First().Messages.Where(x => x.Id == c.State.CurrentMessageId);
            if (messageCandidates == null)
            {
                throw new Exception($"Message \"{c.State.CurrentMessageId}\" is not found in Message Block \"{c.State.CurrentMessageBlock}\" in schema \"{c.State.SchemaName}\" for chat {c.СhatId.ToString()}");
            }
            else if (messageCandidates.Count() != 1)
            {
                throw new Exception($"There are several message blocks \"{c.State.CurrentMessageId}\" in Message Block \"{c.State.CurrentMessageBlock}\" in schema \"{c.State.SchemaName}\" for chat {c.СhatId.ToString()}");
            }
            else
            {
                c.State.CurrentMessage = messageCandidates.First();
            }
            return;
        }
    }
}
