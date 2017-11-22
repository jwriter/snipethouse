using SnippetWebEditor.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SnippetWebEditor.Factory
{
    public class ForView
    {
        public static Tree MakeTree(List<Item> items, List<Note> notes)
        {
            //нужно отсортированные по id items и notes

            //создаём дерево
            var res = new Tree();
            res.Construct(items, notes);

            return res;
        }

        public static DataForView MakeModelThroughNoteId(Tree tree, int noteId)
        {
            DataForView res = new DataForView();
            res.Table = tree.ExrtractTableByItem(noteId);
            var last = GetLastSelectedNode(res.Table);
            if (last != null)
            {
                if (last.Type == NodeType.Folder)
                    res.IsContent = false;
                else
                {
                    Item maybeNote = tree.GetNoteById(noteId).Content;
                    if (maybeNote is Note)
                    {
                        res.IsContent = true;
                        res.Content = ((Note)maybeNote).Content;
                        res.TitleContent = maybeNote.Title;
                    }
                    else
                    {
                        res.IsContent = false;
                    }
                }
            }
            return res;
        }

        public static DataForView MakeModelThroughItemId(Tree tree, int itemId)
        {
            DataForView res = new DataForView();
            res.Table = tree.ExrtractTableByItem(itemId);
            return res;
        }

        private static Node GetLastSelectedNode(List<List<Node>> table)
        {
            List<Node> list = table[table.Count - 1];
            var res = list.Where(n => n.IsSelected).FirstOrDefault();
            return res;
        }

        public static DataForView MakeModelThroughRoot(Tree tree)
        {
            DataForView res = new DataForView();
            res.Table = tree.ExrtractTableByItem(Tree.RootId);
            return res;
        }
    }
}