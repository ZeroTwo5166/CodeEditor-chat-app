using Microsoft.AspNetCore.Mvc;
using backend.Hubs;
using Microsoft.EntityFrameworkCore;


namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HubsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Hub _hub;

        public HubsController(ApplicationDbContext context, Hub hub)
        {
            _context = context;
            _hub = hub;
        }


        [HttpGet("getCodeByUsername/{username}")] 
        public ActionResult<string> GetCodeByUsername(string username)
        {
            // Access the MonacoHub's dictionary
            var connectionDictionary = _hub.GetConnectionDictionary();

            // Find the UserRoomConnection with the matching username
            var userRoomConnection = connectionDictionary.Values.FirstOrDefault(connection => connection.User == username);

            if (userRoomConnection != null)
            {
                // Return the Code if found
                return Ok(userRoomConnection.Code);
            }

            // Handle the case where the username is not found
            return NotFound("User not found");
        }


        [HttpGet("getAllProjectListFromHub")] 
        public ActionResult<IEnumerable<Dictionary<string, string>>> GetAllProjectFromMonacoHub()
        {
            var projectList = _hub.GetProjectDictionary();
            return Ok(projectList);
        }

        [HttpGet("checkRoom/{room}")]
        public IActionResult CheckRoom(string room)
        {
            var containsRoom = _hub.ContainsRoom(room);

            return Ok(new { ContainsRoom = containsRoom });
        }

        [HttpGet("getAllUsersFromHub")]
        public IActionResult GetUsers()
        {
            var userLists = _hub.GetConnectionDictionary();

            return Ok(userLists);
        }

        [HttpPost("disconnectUser/{username}")]
        public IActionResult DisconnectUser(string username)
        {
            _hub.DisconnectUserByUsername(username);

            return Ok(new { Message = $"User '{username}' disconnected successfully" });

        }
    }




}
