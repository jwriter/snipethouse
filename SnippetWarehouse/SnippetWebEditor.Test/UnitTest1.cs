using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnippetWebEditor.Models;
using SnippetWebEditor.Controllers;
using System.Linq;

namespace SnippetWebEditor.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMakeVM()
        {
            var repo = Repository.GetInstance();
            var items = repo.GetItems();
            var notes = repo.GetNotes();
            var vm = new DataForView();
            var sel = vm.MakeData(notes, items);
        }

        [TestMethod]
        public void TestTreeFindParent()
        {
            var tree = new Factory.Tree();
            tree.Construct(null, null);

            int min = 2;
            int max = 13;
            var items = Enumerable.Range(min, max)
                .Select(i => new Item { Id = i })
                .ToList();
            tree.Construct(items, null);

            for (int i = min; i <= max; i++)
            {
                Factory.Tree.Point o = tree.FindPoinById(i);
                Assert.AreEqual(o.Content.Id, i);
            }

            for (int i = min + 1; i < max - 2; i++)
            {
                int removed = i;
                Item link = items[removed];
                items.Remove(link);
                tree.Construct(items, null);
                Factory.Tree.Point o2 = tree.FindPoinById(link.Id);
                Assert.AreEqual(o2.Content.Title, "root");

                for (int j = min; j <= max; j++)
                {
                    Factory.Tree.Point o = tree.FindPoinById(j);
                    if (link.Id != j)
                        Assert.AreEqual(o.Content.Id, j);
                    else
                        Assert.AreEqual(o.Content.Title, "root");
                }

                items.Insert(removed, link);
            }
            

        }
    }
}
