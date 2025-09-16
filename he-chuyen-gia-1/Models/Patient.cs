// Models/Patient.cs
using System.ComponentModel.DataAnnotations;

namespace hechuyengia.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        [Required, StringLength(100)] public string FullName { get; set; } = string.Empty;
        [DataType(DataType.Date)] public DateTime Dob { get; set; }
        [Required] public string Gender { get; set; } = string.Empty;
        public ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
    }
}
