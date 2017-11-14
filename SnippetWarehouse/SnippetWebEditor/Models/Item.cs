namespace SnippetWebEditor.Models
{
    public class Item
    {
        public int Id { get; set; }
        public Item ItemId { get; set; }

        public string Title { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }


}