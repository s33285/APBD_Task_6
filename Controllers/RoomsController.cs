using ApplicationAPBD_6.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationAPBD_6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Room>> GetAll(
            [FromQuery] int? minCapacity,
            [FromQuery] bool? hasProjector,
            [FromQuery] bool? activeOnly)
        {

            var rooms = Database.DataStore.Rooms.AsEnumerable();

            if (minCapacity.HasValue)
            {
                rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);
            }

            if (hasProjector.HasValue)
            {
                rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);
            }

            if (activeOnly.HasValue && activeOnly.Value)
            {
                rooms = rooms.Where(r => r.IsActive);
            }

            return Ok(rooms.ToList());
        }

        [HttpGet("{id:int}")]
        public ActionResult<Room> GetById(int id)
        {
            var room = Database.DataStore.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound($"Room with id {id} not found.");

            return Ok(room);
        }

        [HttpGet("building/{buildingCode}")]
        public ActionResult<List<Room>> GetByBuilding(string buildingCode)
        {
            var rooms = Database.DataStore.Rooms
                .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase)).ToList();

            return Ok(rooms);
        }

        [HttpPost]
        public ActionResult<Room> CreateRoom(Room room)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            room.Id = Database.DataStore.NextRoomId;
            Database.DataStore.Rooms.Add(room);

            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdateRoom(int id, Room updated)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var room = Database.DataStore.Rooms.First(r => r.Id == id);
            if (room == null) return NotFound($"Room with id {id} not found.");

            room.Name = updated.Name;
            room.BuildingCode = updated.BuildingCode;
            room.Floor = updated.Floor;
            room.Capacity = updated.Capacity;
            room.HasProjector = updated.HasProjector;
            room.IsActive = updated.IsActive;

            return Ok(room);
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteRoom(int id)
        {
            var room = Database.DataStore.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound($"Room with id {id} not found.");

            var hasReservation = Database.DataStore.Reservations.Any(r => r.RoomId == id);
            if (hasReservation) return Conflict("Cannot delete room with existing reservations.");

            Database.DataStore.Rooms.Remove(room);
            return NoContent();
        }

    }
}