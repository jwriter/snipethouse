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
            DataForView vm = new DataForView();

            object str = RouteData?.Values["id"]??"";
            int id;
            try
            {
                if (Int32.TryParse((string)str, out id))
                {
                    return View(vm.MakeDataThroughItemId(repo.GetNotes(), repo.GetItems(), id));
                }
            }
            catch { }

            return View(vm.MakeData(repo.GetNotes(), repo.GetItems()));
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

    


}