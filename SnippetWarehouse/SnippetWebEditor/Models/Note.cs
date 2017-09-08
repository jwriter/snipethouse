using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnippetWebEditor.Models
{
    public class Note
    {
        public int Id { get; set; }
        //public Item Parent { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Item ItemId { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }
        public Item ItemId { get; set; }

        public string Title { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}