using backend.Interfaces;
using backend.Models;
using backend.ModelsDTO;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        //DI provides external services on runtime and not compile time
        private readonly IUserRepository _context;

        public UsersController(IUserRepository context)
        {
            _context = context;
        }

        [HttpGet("getidbyusername/{username}")]
        public async Task<IActionResult> GetIdByUsername([FromRoute] string username)
        {
            try
            {
                var user = await _context.GetIdByUsername(username);
                if (user != null)
                {
                    return Ok(new { userId = user.Id });
                }

                else return NotFound("User not found");
            }
            catch (Exception msg)
            {
                return Problem(msg.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO user)
        {
            if (user != null)
            {
                var message = await _context.Login(user);

                // Return appropriate status codes based on the message
                //Can use switch as well
                if (message == "Success")
                {
                    return Ok(new { message = "Login Successful" });
                }
                else if (message == "User not found")
                {
                    return NotFound(message);
                }
                else if (message == "Wrong password")
                {
                    return Unauthorized(message);
                }
                else
                {
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }

            return BadRequest("Invalid request"); // Return BadRequest status code for invalid requests
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var registrationResult = await _context.Register(user);
            // Handle the result based on the returned string
            switch (registrationResult)
            {
                case "Registration successful":
                    return Ok(new { message = "Registration successful" });
                case "Email already exists":
                    return BadRequest("Email already exists");
                case "Username already exists":
                    return BadRequest("Username already exists");
                default:
                    return StatusCode(500, "An error occurred while processing your request");
            }

        }

        [HttpGet("getuserbyid/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            try
            {
                var user = await _context.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);

            }
            catch (Exception msg)
            {

                return Problem(msg.Message);
            }
        }

        //FISHY
        [HttpPost("retrieve")]
        public async Task<ActionResult> GetProfilePic(RetrieveImageDTO userImage)
        {
            if (userImage != null)
            {
                var imageDataURL = await _context.GetProfilePic(userImage);

                // Check the result and return appropriate response
                if (imageDataURL.StartsWith("data:image/jpg;base64,"))
                {
                    return Ok(new { Data = imageDataURL });
                }
                else if (imageDataURL == "Profilepic not found")
                {
                    return NotFound("Profile picture not found");
                }
                else if (imageDataURL == "User not found")
                {
                    return NotFound("User not found");
                }
                else
                {
                    return StatusCode(500, "An error occurred while processing your request");
                }
            }
            else
            {
                return BadRequest("Invalid request");
            }
        }
        //FISHY
        [HttpPost("byteConverter")]
        public async Task<IActionResult> ConvertIFormFileToByteArray([FromForm] IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("Not valid file");
            }

            try
            {
                // Call the asynchronous method using Task.Run to make it synchronous
                byte[] byteArray = await Task.Run(() => _context.ConvertIFormFileToByteArray(file));

                return Ok(byteArray);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error converting file to byte array: {ex.Message}");
            }
        }
    }
}
