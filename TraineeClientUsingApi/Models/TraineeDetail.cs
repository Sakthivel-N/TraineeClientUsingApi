using System.ComponentModel.DataAnnotations;

namespace TraineeClientUsingApi.Models
{
    public class TraineeDetail
    {
        [Key]
        public int TraineeId { get; set; }

        [Required, MaxLength(20)]
        public string TraineeName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required, MaxLength(20)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required, MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(30)]
        public string Interests { get; set; }

        [Required, MaxLength(20)]
        public string Department { get; set; }

        [Required, MaxLength(10)]
        public string BatchCode { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public String Date { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
