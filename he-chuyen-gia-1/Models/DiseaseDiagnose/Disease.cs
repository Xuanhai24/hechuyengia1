using System.ComponentModel.DataAnnotations;

namespace hechuyengia.Models.DiseaseDiagnose
{
    public class Disease
    {
        public string Name { get; set; }
        public double Score { get; set; }
        public List<string> MatchedSymptoms { get; set; }
        public List<string> UnmatchedSymptoms { get; set; }
    }
}
