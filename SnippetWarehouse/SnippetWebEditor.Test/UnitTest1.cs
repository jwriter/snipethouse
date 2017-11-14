using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnippetWebEditor.Models;
using SnippetWebEditor.Controllers;

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
    }
}
