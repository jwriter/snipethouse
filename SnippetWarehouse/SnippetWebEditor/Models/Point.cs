using SnippetWebEditor.Models;
using System.Collections.Generic;

namespace SnippetWebEditor.Models
{
    //элемент дерева
    public class Point
    {
        public Point Parent;
        public List<Point> Children = new List<Point>(4);
        public Item Content;
    }
}