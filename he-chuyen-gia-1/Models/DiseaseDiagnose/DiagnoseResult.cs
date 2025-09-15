using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hechuyengia.Models.DiseaseDiagnose
{
    public class DiagnoseResult
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        public string Symptoms { get; set; } = string.Empty;
        public string Diseases { get; set; } = string.Empty;
        public string MedicinesAdvice { get; set; } = string.Empty;
        public string DoctorAdvice { get; set; } = string.Empty;

        [Column(TypeName = "datetime")]
        public DateTime DiagnoseDate { get; set; }

        public string DoctorName { get; set; } = string.Empty;
    }
}
