// Models/Diagnosis.cs
using System.ComponentModel.DataAnnotations;

namespace hechuyengia.Models
{
    public class Diagnosis
    {
        public int DiagnosisId { get; set; }
        [Required] public string SymptomsCsv { get; set; } = ""; // hoặc JSON
        public string? Result { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
    }
}
