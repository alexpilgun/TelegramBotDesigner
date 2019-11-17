using System;
using System.Collections.Generic;
using System.Text;
using BotDesignerLib;
using LiteDB;
using System.Linq;

namespace ExpenseTrackingBot
{
    public class DBConnector: IDbConnector
    {
        public LiteDatabase db { get; }
        // dbConnection
        // chats

        // expense
        // expense categories
        // google settings

        public DBConnector(string dbFilePath)
        {
            db = new LiteDatabase(dbFilePath);
            //BsonMapper.Global.Entity<Schema>().DbRef(x => x.Chat, "customers");
        }

        public Chat GetChatById(long chatId)
        {
            var allChats = db.GetCollection<Chat>("chats");
            var chat = allChats.Find(x => x.СhatId == chatId).FirstOrDefault();
            return chat;
        }

        public List<Chat> GetAllChats()
        {
            return db.GetCollection<Chat>("chats").FindAll().ToList();
        }

        public void CreateChat(Chat chat)
        {
            var allChats = db.GetCollection<Chat>("chats");
            allChats.Insert(chat);
        }

        public void UpdateChat(Chat chat)
        {
            var allChats = db.GetCollection<Chat>("chats");
            allChats.Update(chat);
        }

        public void DeleteChat(Chat chat)
        {
            // doesn't seem to work
            var allChats = db.GetCollection<Chat>("chats");
            allChats.Delete(chat.СhatId);
        }
    }
}
