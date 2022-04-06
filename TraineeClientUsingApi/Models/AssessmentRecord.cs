using System.ComponentModel.DataAnnotations;

namespace TraineeClientUsingApi.Models
{
    public class AssessmentRecord
    {
        [Key]
        public int RecordId { get; set; }


        [Required]
        public int AssessmentId { get; set; }

        [Required]
        public int TraineeId { get; set; }

        [Required]
        public int Marks { get; set; }

        [Required, MaxLength(20)]
        public string Remarks { get; set; }
    }
}
