using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BotDesignerLib
{
    public class LibDbContext: DbContext
    {
        public DbSet<Chat> Chats { get; set; }
        public LibDbContext ()
        {

        }
    }
}
