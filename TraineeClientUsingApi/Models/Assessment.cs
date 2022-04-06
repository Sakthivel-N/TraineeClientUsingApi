using System.ComponentModel.DataAnnotations;

namespace TraineeClientUsingApi.Models
{
    public class Assessment
    {
        [Key]
        public int AssessmentId { get; set; }

        [Required, MaxLength(20)]
        public string AssessmentName { get; set; }

        [Required]
        public string TrainerName { get; set; }

        [Required, MaxLength(10)]
        public string BatchCode { get; set; }

        [DataType(DataType.Date)]
        [Required, MaxLength(25)]
        public string Date { get; set; }

        [Required, MaxLength(20)]
        public string AssessmentStatus { get; set; }


    }
}
