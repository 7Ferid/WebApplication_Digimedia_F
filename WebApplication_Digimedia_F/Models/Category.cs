namespace WebApplication_Digimedia_F.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Project> Projects { get; set; } = [];
    }
}
