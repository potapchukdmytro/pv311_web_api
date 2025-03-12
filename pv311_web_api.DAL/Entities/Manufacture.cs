using System.ComponentModel.DataAnnotations;

namespace pv311_web_api.DAL.Entities
{
    public class Manufacture
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }
        [MaxLength]
        public string? Description { get; set; }
        [MaxLength(255)]
        public string? Founder { get; set; }
        [MaxLength(255)]
        public string? Director { get; set; }
        public string? Image { get; set; }

        public IEnumerable<Car> Cars { get; set; } = [];
    }
}
