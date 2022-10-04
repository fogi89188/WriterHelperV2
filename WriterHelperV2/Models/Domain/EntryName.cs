namespace WriterHelperV2.Models.Domain
{
    public class EntryName
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }
        public string FirstOrMiddleOrLastName { get; set; } //divides names based on whether they are first, last or middle names
    }
}
