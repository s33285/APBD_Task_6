using ApplicationAPBD_6.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationAPBD_6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Room>> GetAll() 
        {
        
            var rooms = Database.DataStore.Rooms.AsEnumerable();
            return Ok(rooms.ToList());
        }
    }
}
