using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bART_Test.Models
{
    public class Incident
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? IncidentName { get; set; }

        public string? Description { get; set; }

        public int? AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
