namespace Fiver.Azure.Table.Client.OtherLayers
{
    public class Movie : AzureTableEntity
    {
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public string Summary { get; set; }
    }
}
