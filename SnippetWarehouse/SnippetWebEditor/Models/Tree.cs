using SnippetWebEditor.Models;
using System.Collections.Generic;
using System.Linq;

namespace SnippetWebEditor.Models
{
    public class Tree
    {
        public static readonly int RootId = -42;
        public static readonly string RootTitle = "root";

        private Point _root = null;
        private List<Point> _items;
        private List<Point> _notes;
        
        public void Construct(List<Item> Items, List<Note> Notes)
        {
            _root = new Point() { Content = new Item { Id = RootId, ItemId = null, Title = RootTitle } };
            _items = Items?.Select(i => new Point { Content = i }).ToList() ?? (new List<Point>());
            _notes = Notes?.Select(n => new Point { Content = n }).ToList() ?? (new List<Point>());

            foreach (var i in _items)
            {
                var tmp = FindParentItem(i);
                tmp.Children.Add(i);
                i.Parent = tmp;
            }

            foreach (var n in _notes)
            {
                var tmp = FindParentItem(n);
                tmp.Children.Add(n);
                n.Parent = tmp;
            }            
        }
        
        private Point FindParentItem(Point point)
        {
            Item parent = point.Content.ItemId;
            if (parent == null) return _root;
            int parentId = parent.Id;
            return FindPoinById(parentId);
        }

        public Point FindPoinById(int parentId)
        {
            int min = 0;
            int max = _items.Count;
            do
            {
                int nowIndex = min + ((max - min) / 2);
                Point res = _items[nowIndex];
                
                int nowId = res.Content.Id;
                if (parentId == nowId) return res;
                if (parentId > nowId)
                    min = nowIndex;
                else
                    max = nowIndex;

                if (max - min <= 1)
                {
                    if (_items[min].Content.Id == parentId)
                        return _items[min];
                    if (_items[max].Content.Id == parentId)
                        return _items[max];
                    return _root;
                }
            } while (true);
        }

        public List<List<Node>> ExrtractTableByItem(int itemId)
        {
            Point itemContainer = FindPoinById(itemId);
            List<List<Node>> res = new List<List<Node>>();
            Point parent = itemContainer.Parent;
            Point current = itemContainer;
            while (parent != _root)
            {
                res.Insert(0, parent.Children.Select(
                    ch => new Node()
                    {
                        Caption = ch.Content.Title,
                        index = (ch.Content is Item) ? "i" + ch.Content.Id : "n" + ch.Content.Id,
                        IsSelected = ch.Equals(current),
                        Type = (ch.Content is Item) ? NodeType.Folder : NodeType.Article
                    }).ToList());
                current = parent;
                parent = parent.Parent;
            }

            current = itemContainer;
            while (current.Children.Count > 0)
            {
                res.Add(current.Children.Select(
                    ch => new Node()
                    {
                        Caption = ch.Content.Title,
                        index = (ch.Content is Item) ? "i" + ch.Content.Id : "n" + ch.Content.Id,
                        IsSelected = ch.Equals(current.Children[0]),
                        Type = (ch.Content is Item) ? NodeType.Folder : NodeType.Article
                    }).ToList());
                current = current.Children[0];
            }

            return null;
        }

        public Point GetNoteById(int noteId)
        {
            return _notes.Where(n => n.Content.Id == noteId).FirstOrDefault();
        }        
    }
}