using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnippetWebEditor.Models
{
    public class Repository
    {
        private List<Item> items;
        private List<Note> notes;

        public List<Item> GetItems()
        {
            return items;
        }
        public List<Note> GetNotes()
        {
            return notes;
        }
        private Repository()
        {
            Item item1 = new Item { Id = 1, ItemId = null, Title = "1" };
            Item item2 = new Item { Id = 2, ItemId = item1, Title = "2" };
            Item item3 = new Item { Id = 3, ItemId = item1, Title = "3" };
            Item item4 = new Item { Id = 4, ItemId = item2, Title = "4" };
            Item item5 = new Item { Id = 5, ItemId = item4, Title = "5" };
            Item item6 = new Item { Id = 6, ItemId = item4, Title = "6" };
            Item item7 = new Item { Id = 7, ItemId = item6, Title = "7" };
            Item item8 = new Item { Id = 8, ItemId = item6, Title = "8" };
            Item item9 = new Item { Id = 9, ItemId = null, Title = "9" };
            Item item10 = new Item { Id = 10, ItemId = item9, Title = "10" };
            Item item11 = new Item { Id = 11, ItemId = item10, Title = "11" };
            Item item12 = new Item { Id = 12, ItemId = item10, Title = "12" };

            items = new List<Item>()
            {
                item1, item2, item3, item4, item5, item6,
                item7, item8, item9, item10,  item11, item12,
            };

            Note note1 = new Note { Id = 1, ItemId = item5, Content = "111111111111111111111", Title = "Note 1" };
            Note note2 = new Note { Id = 2, ItemId = item7, Content = "222222222222222222222", Title = "Note 2" };
            Note note3 = new Note { Id = 3, ItemId = item8, Content = "333333333333333333333", Title = "Note 3" };
            Note note4 = new Note { Id = 4, ItemId = item11, Content = "444444444444444444444", Title = "Note 4" };
            Note note5 = new Note { Id = 5, ItemId = item12, Content = "555555555555555555555", Title = "Note 5" };

            notes = new List<Note>() { note1, note2, note3, note4, note5, };
        }

        private static Repository _instance;
        public static Repository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Repository();
            }
            return _instance;
        }
    }
}