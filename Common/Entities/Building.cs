using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities;

public class Building
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BuildingId { get; set; }
    [Required]
    public string Address { get; set; }
    
    public string Name { get; set; }
    
    public virtual ICollection<Room> Rooms { get; set; }
}