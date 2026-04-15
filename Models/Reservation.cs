using System.ComponentModel.DataAnnotations;

namespace ApplicationAPBD_6.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int RoomId { get; set; }

        [Required] 
        public string OrganizerName { get; set; } = string.Empty;
               
        [Required]
        public string Topic { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }

        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
