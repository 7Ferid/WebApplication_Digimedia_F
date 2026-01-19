namespace WebApplication_Digimedia_F.ViewModels.ProjectViewModels
{
    public class ProjectCreateVM
    {
    
        public string Name { get; set; } = string.Empty;
        public IFormFile Image { get; set; } = null!;

        public int CategoryId { get; set; } 
    }
}
