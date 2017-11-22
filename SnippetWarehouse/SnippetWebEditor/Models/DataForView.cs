using System.Collections.Generic;

namespace SnippetWebEditor.Models
{
    /// <summary>
    /// модель для Table View
    /// </summary>
    public class DataForView
    {
        public string MainTitle { get; set; }
        public List<List<Node>> Table { get; set; }
        public bool IsContent { get; set; }
        public string TitleContent { get; set; }
        public string Content { get; set; }
    }
}