using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Interfaces
{
    public interface IUserController
    {
        public Task<IActionResult> Register([FromBody] UserDto user);
        public Task<IActionResult> Login([FromBody] Login user);
        public Task<ActionResult> GetProfilePic(RetrieveImage userImage);
        public Task<IActionResult> GetId([FromBody] Getid id);
        public Task<IActionResult> GetIdByUsername([FromRoute] string username);
        public IActionResult ConvertIFormFileToByteArray([FromForm] IFormFile file);
        public byte[] ConvertIFormFileToByteArrayInternal(IFormFile file);
    }
}
