using hechuyengia.Models;
using System.ComponentModel.DataAnnotations;

public class Doctor
{
    public int DoctorId { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }

    [Required, StringLength(100)]
    public string FullName { get; set; } = "";

    [EmailAddress]
    public string? Email { get; set; }   // 👈 thêm

    [Phone]
    public string? Phone { get; set; }   // 👈 thêm

    public string? Specialty { get; set; }

    public ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
}