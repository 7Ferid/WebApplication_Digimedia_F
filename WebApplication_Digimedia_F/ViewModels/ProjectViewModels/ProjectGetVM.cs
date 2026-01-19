using WebApplication_Digimedia_F.Models;

namespace WebApplication_Digimedia_F.ViewModels.ProjectViewModels
{
    public class ProjectGetVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;
 
    }
}
