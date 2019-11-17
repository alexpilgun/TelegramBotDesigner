using System;
using System.Collections.Generic;
using System.Text;

namespace BotDesignerLib
{
    public interface IDbConnector
    {
        Chat GetChatById(long chatId);
        List<Chat> GetAllChats();
        void CreateChat(Chat chat);
        void UpdateChat(Chat chat);
        void DeleteChat(Chat chat);
    }
}
