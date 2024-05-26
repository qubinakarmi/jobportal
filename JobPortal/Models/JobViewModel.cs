using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobPortal.Models
{
    public class JobViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string OrganizationName { get; set; }
        public int OrganizationId { get; set; }
        public string CreatedBy { get; set; }
    }
    public class PaginatedJobViewModel : PageModel
    {
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 10; 
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public List<JobViewModel> Data { get; set;}
    } 
}
