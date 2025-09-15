// Models/Report.cs (ví dụ)
namespace hechuyengia.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public string Type { get; set; } = string.Empty;    // tránh CS8618
        public string Content { get; set; } = string.Empty; // tránh CS8618

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
