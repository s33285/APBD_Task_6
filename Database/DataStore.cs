using ApplicationAPBD_6.Models;

namespace ApplicationAPBD_6.Database
{
    public static class DataStore
    {

        public static List<Room> Rooms { get; } = new()
        {
        new Room { Id = 1, Name = "Lab 101", BuildingCode = "A", Floor = 1, Capacity = 20, HasProjector = true,  IsActive = true },
        new Room { Id = 2, Name = "Lab 204", BuildingCode = "B", Floor = 2, Capacity = 24, HasProjector = true,  IsActive = true },
        new Room { Id = 3, Name = "Conference 1", BuildingCode = "A", Floor = 3, Capacity = 40, HasProjector = false, IsActive = true },
        new Room { Id = 4, Name = "Old Room", BuildingCode = "C", Floor = 0, Capacity = 15, HasProjector = false, IsActive = false },
        new Room { Id = 5, Name = "Workshop Room", BuildingCode = "B", Floor = 1, Capacity = 30, HasProjector = true, IsActive = true }

        };

        public static List<Reservation> Reservations { get; } = new()
        {
        new Reservation
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "Jan Nowak",
            Topic = "C# Basics",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(9, 0, 0),
            EndTime = new TimeOnly(11, 0, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 2,
            RoomId = 2,
            OrganizerName = "Anna Kowalska",
            Topic = "HTTP and REST Workshop",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(10, 0, 0),
            EndTime = new TimeOnly(12, 30, 0),
            Status = "planned"
        },
        new Reservation
        {
            Id = 3,
            RoomId = 3,
            OrganizerName = "Piotr Zielinski",
            Topic = "Design Patterns",
            Date = new DateOnly(2026, 5, 11),
            StartTime = new TimeOnly(13, 0, 0),
            EndTime = new TimeOnly(15, 0, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 4,
            RoomId = 5,
            OrganizerName = "Maria Nowak",
            Topic = "Unit Testing",
            Date = new DateOnly(2026, 5, 12),
            StartTime = new TimeOnly(9, 30, 0),
            EndTime = new TimeOnly(11, 30, 0),
            Status = "cancelled"
        }
        };

        public static int NextRoomId => Rooms.Count > 0 ? Rooms.Max(r => r.Id) + 1 : 1;
        public static int NextReservationId => Reservations.Count > 0 ? Reservations.Max(r => r.Id) + 1 : 1;
    }
}
