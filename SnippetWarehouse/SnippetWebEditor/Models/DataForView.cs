using System;
using System.Collections.Generic;
using System.Linq;

namespace SnippetWebEditor.Models
{
    public class DataForView
    {
        public List<ChoosedList> Table { get; set; } = new List<ChoosedList>();
        public List<Note> Notes { get; set; } = new List<Note>();
        public string Title { get; set; }
        public string Text { get; set; }

        public DataForView MakeData(List<Note> notes, List<Item> items)
        {
            var choose = items.Where(i => i.ItemId == null).First();
            return MakeDataThroughItem(notes, items, choose);
        }

        public DataForView MakeData(List<Note> notes, List<Item> items, int itemId, int noteId)
        {


            return null;
        }

        public DataForView MakeDataThroughItemId(List<Note> notes, List<Item> items, int itemId)
        {
            var currents = items.Where(i => i.Id == itemId).ToList();
            Item choose;
            if (currents.Count == 1)
                choose = currents[0];
            else if (currents.Count == 0)
            {
                choose = items.Where(i => i.ItemId == null).First();
            }
            else
                choose = items.Where(i => i.ItemId == null).First();
            return MakeDataThroughItem(notes, items, choose);
        }

        public DataForView MakeDataThroughItem(List<Note> notes, List<Item> items, Item current)
        {
            if (!items.Contains(current))
                throw new ArgumentException();

            DataForView res = new DataForView();

            Item parent = null;
            Item tmp = current;
            //get parents
            do
            {
                parent = tmp.ItemId;
                List<Item> childs = items.Where(i => i.ItemId == parent).ToList();
                if (childs.Count > 0)
                {
                    ChoosedList cl = new ChoosedList(childs, childs.IndexOf(tmp));
                    res.Table.Add(cl);
                }
                tmp = parent;
            } while (parent != null);
            res.Table.Reverse();
            parent = current;
            List<Item> nodes = new List<Item>();
            //get childs
            do
            {
                List<Item> column = items.Where(i => i.ItemId == parent).ToList();
                nodes.AddRange(column);
                if (column.Count > 0)
                {
                    ChoosedList cl = new ChoosedList(column, 0);
                    res.Table.Add(cl);
                }
                else
                    break;
                parent = column[0];
            } while (true);
            //get notes
            res.Notes = notes.Where(n => nodes.Contains(n.ItemId)).ToList();
            if (res.Notes.Count > 0)
            {
                res.Title = res.Notes[0].Title;
                res.Text = res.Notes[0].Content;
            }
            else
            {
                res.Title = "no title";
                res.Text = "no content";
            }

            return res;
        }

        public DataForView MakeDataThroughNoteId(List<Note> notes, List<Item> items, int noteId)
        {
            var note = notes.Where(x => x.Id == noteId).FirstOrDefault();
            if (note == null)
                return MakeData(notes, items);
            var parent = items.Where(x => x.Id == note.ItemId.Id).FirstOrDefault();
            if (parent == null)
                return MakeData(notes, items);
            var res = MakeDataThroughItem(notes, items, parent);

            return res;
        }

        public override string ToString()
        {
            string res = "";
            foreach (var item in Table)
            {
                for (int i = 0; i < item.Parts.Count; i++)
                {
                    res += item.Parts[i].ToString();
                    if (i == item.ChoosedPart)
                        res += "_";
                    res += "; ";
                }
                res += Environment.NewLine;
            }
            res += "Title" + Title + Environment.NewLine;
            res += "Text" + Text + Environment.NewLine;
            return res;
        }

    }


}