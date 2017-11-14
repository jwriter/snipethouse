using System.Collections.Generic;

namespace SnippetWebEditor.Models
{
    public class ChoosedList
    {
        public List<Item> Parts { get; set; }
        public int ChoosedPart { get; set; }

        public ChoosedList(List<Item> list, int place)
        {
            Parts = list;
            ChoosedPart = place;
        }
    }


}