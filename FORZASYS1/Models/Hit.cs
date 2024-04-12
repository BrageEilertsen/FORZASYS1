namespace FORZASYS1.Models
{
    public class Hit
    {
        public Source Source { get; set; }
        public string _id { get; set; } // Ensure this is the property for the Elasticsearch _id
    }
}