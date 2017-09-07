﻿using SnippetWebEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SnippetWebEditor.Controllers
{
    public class HomeController : Controller
    {

        ViewModel vm;
        public HomeController()
        {
            
        }


        private ViewModel MakeVM(List<Note> notes, List<Item> items, int itemId = 0, int noteId = 0)
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
                    }
                    tmp = parent;
                } while (parent != null);
            }
            else
            {

            }


            return res;
        }

        

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
        public List<ChossedList> table { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

    }

    public class ChossedList
    {
        public List<int> Parts { get; set; }
        public int ChoosedPart { get; set; }

        public ChossedList(List<int> list, int place) 
        {
            Parts = list;
            ChoosedPart = place;
        }
    }


}