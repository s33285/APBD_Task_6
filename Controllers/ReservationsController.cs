using ApplicationAPBD_6.Models;
using Microsoft.AspNetCore.Mvc;
using System.Buffers;

namespace ApplicationAPBD_6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Reservation>> GetAll(
            [FromQuery] DateOnly? date,
            [FromQuery] string? status,
            [FromQuery] int? roomId)
        {

            var res = Database.DataStore.Reservations.AsEnumerable();

            if (date.HasValue)
            {
                res = res.Where(r => r.Date == date.Value);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                res = res.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            if (roomId.HasValue)
            {
                res = res.Where(r => r.RoomId == roomId.Value);
            }

            return Ok(res.ToList());
        }

        [HttpGet("{id:int}")]
        public ActionResult<Reservation> GetById(int id)
        {
            var res = Database.DataStore.Reservations.FirstOrDefault(r => r.Id == id);
            if (res == null) return NotFound($"Reservation with id {id} not found.");

            return Ok(res);
        }

        [HttpPost]
        public ActionResult<Reservation> CreateReservation(Reservation reservation)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            if (reservation.EndTime <= reservation.StartTime)
            {
                return BadRequest("EndTime must be later than StartTime.");
            }

            var room = Database.DataStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
            if (room == null) return Conflict("Room does not exist.");

            if (!room.IsActive) return Conflict("Cannot create reservation for inactive room.");

            if (HasOverlap(reservation, null))
                return Conflict("Reservation overlaps with an existing reservation.");

            reservation.Id = Database.DataStore.NextReservationId;
            Database.DataStore.Reservations.Add(reservation);

            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdateReservation(int id, Reservation updated)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            if (updated.EndTime <= updated.StartTime) return BadRequest("EndTime must be later than StartTime.");

            var existing = Database.DataStore.Reservations.FirstOrDefault(r => r.Id == id);
            if (existing == null) return NotFound($"Reservation with id {id} not found.");

            var room = Database.DataStore.Rooms.FirstOrDefault(r => r.Id == updated.RoomId);
            if (room == null) return Conflict("Room does not exist.");

            if (!room.IsActive) return Conflict("Cannot update reservation for inactive room.");

            if (HasOverlap(updated, id)) return Conflict("Reservation overlaps with an existing reservation.");

            existing.RoomId = updated.RoomId;
            existing.OrganizerName = updated.OrganizerName;
            existing.Topic = updated.Topic;
            existing.Date = updated.Date;
            existing.StartTime = updated.StartTime;
            existing.EndTime = updated.EndTime;
            existing.Status = updated.Status;

            return Ok(existing);
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteReservation(int id)
        {
            var res = Database.DataStore.Reservations.FirstOrDefault(r => r.Id == id);
            if (res == null) return NotFound($"Room with id {id} not found.");

            Database.DataStore.Reservations.Remove(res);
            return NoContent();
        }

        private bool HasOverlap(Reservation cand, int? ignoreId)
        {
            return Database.DataStore.Reservations.Any(r =>
            (!ignoreId.HasValue || r.Id != ignoreId.Value) &&
            r.RoomId == cand.RoomId &&
            r.Date == cand.Date &&
            cand.StartTime < r.EndTime &&
            cand.EndTime > r.StartTime);

        }
    }
}