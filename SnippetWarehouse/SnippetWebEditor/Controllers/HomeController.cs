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
        public string Title { get; set; }
        public string Text { get; set; }


        public ViewModel MakeVM(List<Note> notes, List<Item> items, int itemId = 1, int noteId = 1)
        {
            ViewModel res = new ViewModel();
            if (itemId > 0)
            {
                Item parent = null;
                Item current = items.Where(i => i.Id == itemId).FirstOrDefault();
                if (current == null)
                {
                    //TODO
                    throw new Exception();
                }
                Item tmp = current;
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
                do
                {
                    List<Item> childs = items.Where(i => i.ItemId == parent).ToList();
                    if (childs.Count > 0)
                    {
                        ChossedList cl = new ChossedList(childs, 0);
                        res.Table.Add(cl);
                    }
                    else
                        break;
                    parent = childs[0];
                } while (true);
            }
            else
            {

            }


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