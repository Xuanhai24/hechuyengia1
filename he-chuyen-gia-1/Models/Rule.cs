//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace hechuyengia.Models
//{
//    public class Rule
//    {
//        public int RuleId { get; set; }

//        [Required]
//        public int SymptomId { get; set; }

//        [Required]
//        public int DiseaseId { get; set; }

//        // Nên dùng decimal thay cho double để lưu trong DB chính xác hơn
//        [Range(0, 1)]
//        [Column(TypeName = "decimal(5,2)")]
//        public decimal Weight { get; set; } = 1m; // 0..1 (CF / độ tin cậy)

//        [MaxLength(500)]
//        public string? Description { get; set; }

//        // Nav
//        public Symptom Symptom { get; set; } = null!;
//        //public Disease Disease { get; set; } = null!;
//    }
//}
