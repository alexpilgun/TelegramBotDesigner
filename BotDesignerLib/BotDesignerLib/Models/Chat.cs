﻿using System;
using System.Linq;

namespace BotDesignerLib
{
    public class Chat
    {
        public readonly long chatId;
        
        public ChatState State { get; set; }

        public Chat(long chatIdInput)
        {
            chatId = chatIdInput;
            State = new ChatState(this);
        }
    }

    public class ChatState
    {
        public Chat Chat { get; set; }
        public MessageBlock CurrentMessageBlock { get; set; }
        public Message CurrentMessage { get; set; }
        public DataContext DataContext { get; set; }
        public bool WaitForUserTransition { get; set; }
        public bool HasBeenAtLastMessage { get; set; }

        public ChatState(Chat newChat)
        {
            Chat = newChat;
            DataContext = new DataContext();
            WaitForUserTransition = false;
        }
    }

    public class DataContext
    {
        public DataContext() { }
    }
}
