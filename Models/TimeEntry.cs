using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TempusFujit.Models
{
    public class TimeEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public DateTime StartingTime { get; set; }
        //If null it is draft
        public DateTime? EndingTime { get; set; }
    }
}
