using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BotDesignerLib
{
    public class DataContext
    {
        [Key]
        public string Id { get; set; }
        public DataContext()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
