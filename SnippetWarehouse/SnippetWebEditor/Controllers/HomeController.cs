using SnippetWebEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SnippetWebEditor.Controllers
{
    public class HomeController : Controller
    {        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Table()
        {
            Repository repo = new Repository();
            ViewModel vm = new ViewModel();

            object str = RouteData.Values["id"];
            int id;
            try
            {
                if (Int32.TryParse((string)str, out id))
                {
                    return View(vm.MakeVmItemId(repo.GetNotes(), repo.GetItems(), id));
                }
            }
            catch { }

            return View(vm.MakeVM(repo.GetNotes(), repo.GetItems()));
        }

        public ActionResult AddNote()
        {
            object str = RouteData.Values["id"];
            return View();
        }

        public ActionResult AddItem()
        {
            object str = RouteData.Values["id"];
            
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    public class ViewModel
    {
        public List<ChossedList> Table { get; set; } = new List<ChossedList>();
        public List<Note> Notes { get; set; } = new List<Note>();
        public string Title { get; set; }
        public string Text { get; set; }

        public ViewModel MakeVM(List<Note> notes, List<Item> items)
        {
            var choose = items.Where(i => i.ItemId == null).First();
            return MakeVmForItem(notes, items, choose);
        }

        public ViewModel MakeVM(List<Note> notes, List<Item> items, int itemId, int noteId)
        {


            return null;
        }

        public ViewModel MakeVmItemId(List<Note> notes, List<Item> items, int itemId)
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
            return MakeVmForItem(notes, items, choose);
        }

        public ViewModel MakeVmForItem(List<Note> notes, List<Item> items, Item current)
        {
            if (!items.Contains(current))
                throw new  ArgumentException();

            ViewModel res = new ViewModel();            
            
            Item parent = null;
            Item tmp = current;
            //get parents
            do
            {
                parent = tmp.ItemId;
                List<Item> childs = items.Where(i => i.ItemId == parent).ToList();
                if (childs.Count > 0)
                {
                    ChossedList cl = new ChossedList(childs, childs.IndexOf(tmp));
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
                    ChossedList cl = new ChossedList(column, 0);
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

        public ViewModel MakeVmNoteId(List<Note> notes, List<Item> items, int noteId)
        {
            var note = notes.Where(x => x.Id == noteId).FirstOrDefault();
            if (note == null)
                return MakeVM(notes, items);
            var parent = items.Where(x => x.Id == note.ItemId.Id).FirstOrDefault();
            if (parent == null)
                return MakeVM(notes, items);
            var res = MakeVmForItem(notes, items, parent);

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

    public class ChossedList
    {
        public List<Item> Parts { get; set; }
        public int ChoosedPart { get; set; }

        public ChossedList(List<Item> list, int place) 
        {
            Parts = list;
            ChoosedPart = place;
        }
    }


}