using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hechuyengia.Models.DiseaseDiagnose
{
    public class DiagnoseResult
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Mã BN đang để trống !")]
        public int PatientId { get; set; }

        public Patient? Patient { get; set; }

        [Required(ErrorMessage = "Kết luận triệu chứng đang để trống !")]
        public string Symptoms { get; set; } = string.Empty;
        [Required(ErrorMessage ="Kết luận bệnh đang để trống !")]
        public string Diseases { get; set; } = string.Empty;
        [Required(ErrorMessage ="Đề xuất thuốc đang để trống ! ")]
        public string MedicinesAdvice { get; set; } = string.Empty;
        public string DoctorAdvice { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime DiagnoseDate { get; set; }
        public string DoctorName { get; set; } = string.Empty;
    }
}
