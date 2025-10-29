using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Person_pasport_One_to_one.Models
{
    public class Passport
    {
        [Key]
        [ForeignKey("Person")]

        public int PersonId { get; set; }

        [Required]
        [StringLength(20)]
        public string PassportNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; }

        [Required]
        [DataType(DataType.Date)]

        public DateTime ExpiryDate { get; set; }

        [JsonIgnore]
        public virtual Person? Person { get; set; }
    }
}
