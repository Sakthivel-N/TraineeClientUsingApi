using System.ComponentModel.DataAnnotations;

namespace TraineeClientUsingApi.Models
{
    public class AdminDetail
    {
        [Key]
        public int AdminId { get; set; }

        [Required, MaxLength(20)]
        public string AdminName { get; set; }

        [Required]
        public string Designation { get; set; }

        [DataType(DataType.Password)]
        [Required, MaxLength(20)]
        public string Password { get; set; }
    }
}
