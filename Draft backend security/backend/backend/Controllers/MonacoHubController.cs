using backend.Hub;
using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonacoHubController : Controller, IMonacoHubController
    {
        private readonly ApplicationDbContext _context;
        private readonly MonacoHub _monacoHub;

        public MonacoHubController(ApplicationDbContext context, MonacoHub monacoHub)
        {
            _context = context;
            _monacoHub = monacoHub;
        }

        [HttpGet("getCodeByUsername/{username}")] //from monacoHub
        public ActionResult<string> GetCodeByUsername(string username)
        {
            // Access the MonacoHub's dictionary
            var connectionDictionary = _monacoHub.GetConnectionDictionary();

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


        [HttpGet("getAllProjectListFromHub")] //form monacoHub
        public ActionResult<IEnumerable<Dictionary<string, string>>> GetAllProjectFromMonacoHub()
        {
            var projectList = _monacoHub.GetProjectDictionary();
            return Ok(projectList);
        }

        [HttpGet("checkRoom/{room}")]
        public IActionResult CheckRoom(string room)
        {
            var containsRoom = _monacoHub.ContainsRoom(room);

            return Ok(new { ContainsRoom = containsRoom });
        }

        [HttpGet("getAllUsersFromHub")]
        public IActionResult GetUsers()
        {
            var userLists = _monacoHub.GetConnectionDictionary();

            return Ok(userLists);
        }

        [HttpPost("disconnectUser/{username}")]
        public IActionResult DisconnectUser(string username)
        {
            _monacoHub.DisconnectUserByUsername(username);

            return Ok(new { Message = $"User '{username}' disconnected successfully" });

        }

    }
}
