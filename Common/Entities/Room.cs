using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities;

public class Room
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RoomId { get; set; }
    
    [Required]
    public string RoomName { get; set; }
    
    public virtual ICollection<Booking> Bookings { get; set; }
    
    public virtual Building Building { get; set; }
}