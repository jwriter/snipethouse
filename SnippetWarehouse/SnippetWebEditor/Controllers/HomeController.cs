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
            var repo = Repository.GetInstance();
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

        [HttpGet]
        public ActionResult AddNote()
        {
            ViewBag.ParentId = RouteData.Values["id"];
            return View();
        }
        [HttpPost]
        [ActionName("AddNote")]
        public ActionResult AddNotePost()
        {
            string parentId = Request.Form["ParentId"];
            string text = Request.Form["NoteText"];
            string title = Request.Form["NoteTitle"];

            int parent;
            if (Int32.TryParse(parentId, out parent))
            {
                var repo = Repository.GetInstance();
                var notes = repo.GetNotes();
                var items = repo.GetItems();
                int max = notes.Max(m => m.Id);
                max++;
                Item parentItem = items.Where(i => i.Id == parent).First();
                notes.Add(new Note { Id = 1, ItemId = parentItem, Content = text, Title = title });
                return RedirectToAction("Table", "Home", new { id = parent });
            }
            return RedirectToAction("Table", "Home");
        }

        public ActionResult DeleteNote()
        {
            object str = RouteData.Values["id"];
            int id;
            try
            {
                if (Int32.TryParse((string)str, out id))
                {
                    var repo = Repository.GetInstance();
                    var notes = repo.GetNotes();

                    Note note = notes.Where(i => i.Id == id).FirstOrDefault();
                    Item item = note.ItemId;
                    
                    if (note != null)
                    {
                        notes.Remove(note);
                        return RedirectToAction("Table", "Home", note.Id);
                    }
                    return RedirectToAction("Table", "Home", id);
                }
            }
            catch { }

            return RedirectToAction("Table", "Home");
        }

        public ActionResult EditNote()
        {
            object tmp = RouteData.Values["id"];
            int id;
            try
            {
                if (Int32.TryParse((string)tmp, out id))
                {
                    ViewBag.Id = tmp;
                    var repo = Repository.GetInstance();
                    var notes = repo.GetNotes();
                    var note = notes.Where(i => i.Id == id).FirstOrDefault();
                    ViewBag.NoteTitle = note.Title;
                    ViewBag.NoteText = note.Content;
                    return View();
                }
            }
            catch { }
            return RedirectToAction("Table", "Home");
            
        }
        [HttpPost]
        [ActionName("EditNote")]
        public ActionResult EditNotePost()
        {
            string tmp = Request.Form["Id"];
            string text = Request.Form["NoteText"];
            string title = Request.Form["NoteTitle"];

            int id;
            if (Int32.TryParse(tmp, out id))
            {
                var repo = Repository.GetInstance();
                var notes = repo.GetNotes();
                var items = repo.GetItems();
                Note note = notes.Where(i => i.Id == id).First();
                note.Title = title;
                note.Content = text;
                return RedirectToAction("Table", "Home", new { id = note.ItemId.Id });
            }
            return RedirectToAction("Table", "Home");
        }

        [HttpGet]
        public ActionResult AddItem()
        {
            ViewBag.ParentId = RouteData.Values["id"];
            return View();
        }
        [HttpPost]
        [ActionName("AddItem")]
        public ActionResult AddItemPost()
        {
            string parentId = Request.Form["ParentId"];
            string text = Request.Form["ItemText"];

            int parent;
            if (Int32.TryParse(parentId, out parent))
            {
                var repo = Repository.GetInstance();
                var items = repo.GetItems();
                int max = items.Max(m => m.Id);
                max++;
                items.Add(new Item { Id = max , ItemId = items.Where(i=>i.Id==parent).First(), Title = text});
                return RedirectToAction("Table", "Home", new { id = max });
            }           
            return RedirectToAction("Table", "Home");
        }

        public ActionResult DeleteItem()
        {
            object str = RouteData.Values["id"];
            int id;
            try
            {
                if (Int32.TryParse((string)str, out id))
                {
                    var repo = Repository.GetInstance();
                    var items = repo.GetItems();
                    Item item = items.Where(i => i.Id == id).FirstOrDefault();
                    Item parent = item.ItemId;
                    if (item != null)
                    {
                        if (items.Count(i => i.ItemId == item) == 0)
                        {
                            items.Remove(item);
                            return RedirectToAction("Table", "Home", parent.Id);
                        }
                    }
                    return RedirectToAction("Table", "Home", id);
                }
            }
            catch { }

            return RedirectToAction("Table", "Home");
        }
        [HttpGet]
        public ActionResult EditItem()
        {
            object tmp = RouteData.Values["id"];
            int id;
            try
            {
                if (Int32.TryParse((string)tmp, out id))
                {
                    ViewBag.ParentId = tmp;
                    var repo = Repository.GetInstance();
                    var items = repo.GetItems();
                    ViewBag.Content = items.Where(i => i.Id == id).FirstOrDefault().Title;
                    return View();
                }
            }
            catch { }
            return RedirectToAction("Table", "Home");
        }
        [HttpPost]
        [ActionName("EditItem")]
        public ActionResult EditItemPost()
        {
            string tmp = Request.Form["ParentId"];
            string text = Request.Form["ItemText"];

            int id;
            if (Int32.TryParse(tmp, out id))
            {
                var repo = Repository.GetInstance();
                var items = repo.GetItems();
                var itemt = items.Where(i => i.Id == id).FirstOrDefault();
                itemt.Title = text;
                return RedirectToAction("Table", "Home", new { id = id });
            }
            return RedirectToAction("Table", "Home");
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