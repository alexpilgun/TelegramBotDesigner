using System;
using System.Collections.Generic;
using System.Text;

namespace BotDesignerLib
{
    public class DbConnector
    {
        public Guid Id;
        public List<ChatState> states;
        public List<Chat> chats;

        public DbConnector()
        {
            Id = Guid.NewGuid();
            chats = new List<Chat>();
            states = new List<ChatState> ();
        }

    }
}
