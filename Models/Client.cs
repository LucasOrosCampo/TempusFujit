using System.ComponentModel.DataAnnotations;

namespace TempusFujit.Models
{
    public class Client
    {
        public Client(string name, string description = null)
        {
            Name = name;
            Description = description;
        }

        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
