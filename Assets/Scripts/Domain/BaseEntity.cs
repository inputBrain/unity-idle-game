namespace Domain
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public ReactiveProperty<string> ImageResourcesPath = new();
    }
}