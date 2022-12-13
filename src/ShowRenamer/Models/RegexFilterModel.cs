namespace ShowRenamer.Models
{
    public class RegexFilterModel
    {
        public string Regex { get; set; }

        public string Name { get; set; }

        public string Display => $"{Name} ({Regex})";
    }
}
