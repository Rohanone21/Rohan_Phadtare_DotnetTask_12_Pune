using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Person_pasport_One_to_one.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int PersonId { get; set; }   

        [Required]
        [StringLength(100)]
        [JsonPropertyName("personName")]
        public string PersonName { get; set; }=string.Empty;


      
        public virtual Passport? Passport { get; set; }

    }
}
