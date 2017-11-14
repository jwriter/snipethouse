using SnippetWebEditor.Models;
using System;
using System.Collections.Generic;
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
            return null;
        }

        public static DataForView MakeModelThroughItemId(Tree tree, int itemId)
        {
            return null;
        }

        public static DataForView MakeModelThroughRoot(Tree tree)
        {
            return null;
        }
    }

    public class Tree
    {
        private Point _root = null;
        private List<Point> _items;
        private List<Point> _notes;
        
        //TODO: linq
        public void Construct(List<Item> Items, List<Note> Notes)
        {
            _root = new Point();
            _items = new List<Point>(Items.Count);
            for (int i = 0; i < Items.Count; i++)
            {
                _items[i].Content = Items[i];
            }
            _notes = new List<Point>(Notes.Count);
            for (int i = 0; i < Notes.Count; i++)
            {
                _notes[i].Content = Notes[i];
            }
            
            for (int i = 0; i < Items.Count; i++)
            {
                var tmp = FindParentItem(_items[i]);
                tmp.Children.Add(_items[i]);
                _items[i].Parent = tmp;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                var tmp = FindParentItem(_items[i]);
                tmp.Children.Add(_items[i]);
                _items[i].Parent = tmp;
            }
            for (int i = 0; i < Notes.Count; i++)
            {
                var tmp = FindParentItem(_notes[i]);
                tmp.Children.Add(_notes[i]);
                _notes[i].Parent = tmp;
            }
        }

        //TODO: test 
        private Point FindParentItem(Point point)
        {
            Item wanted = point.Content.ItemId;
            if (wanted == null) return _root;
            int parentId = wanted.Id;
            return FindPoinById(parentId);
        }

        private Point FindPoinById(int parentId)
        {
            int min = 0;
            int max = _items.Count;
            do
            {
                int nowIndex = min + ((max - min) / 2);
                Point res = _items[max / 2];

                int nowId = res.Content.Id;
                if (parentId == nowId) return res;
                if (parentId > nowId)
                    min = nowIndex;
                else
                    max = nowIndex;

                if (min == max)
                {
                    return _root;
                }

            } while (true);
        }

        public object FindItem(int itemId)
        {
            Point itemContainer = FindPoinById(itemId);
            List<List<Point>> list = new List<List<Point>>();
            Point parent = itemContainer.Parent;
            while (parent != _root)
            {
                list.Insert(0, parent.Children);
                parent = parent.Parent;
            } 


            return null;
        }

        //элемент дерева
        class Point
        {
            public Point Parent;
            public List<Point> Children = new List<Point>(4);
            public Item Content;
        }
        
    }

    /// <summary>
    /// модель для Table View
    /// </summary>
    public class DataForView
    {
        public string MainTitle { get; }
        public List<List<Node>> Table { get; }
        public bool IsContent { get; }
        public string TitleContent { get; }
        public string Content { get; }
    }

    public class Node
    {
        public bool IsSelected { get; }
        public NodeType Type { get; }
        public string Caption { get; }
        public string index { get; }
    }

    public enum NodeType
    {
        Folder, Article,
    }
}