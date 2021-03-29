namespace ShowRenamer.Models
{
    public class BrowseFilterModel
    {
        public string Name { get; set; }

        public string Pattern { get; set; }

        public string FullName => $"{Name} ({Pattern})";
    }
}
