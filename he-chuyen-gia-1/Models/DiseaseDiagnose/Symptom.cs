using System.ComponentModel.DataAnnotations;

namespace hechuyengia.Models.DiseaseDiagnose
{
    public class Symptom
    {
        [Required]
        public string Name { get; set; }
    }
}
