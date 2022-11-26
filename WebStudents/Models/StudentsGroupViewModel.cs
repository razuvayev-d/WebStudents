using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebDB.Models
{
    public class StudentsView
    {
        public List<Student>? Students { get; set; }
        public SelectList? Groups { get; set; }
        public string? StudentsGroups { get; set; }
        public string? SearchString { get; set; }
    }
}
