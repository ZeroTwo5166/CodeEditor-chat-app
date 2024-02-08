using Microsoft.AspNetCore.Mvc;

namespace backend.Interfaces
{
    public interface IMonacoHubController
    {
        public ActionResult<string> GetCodeByUsername(string username);
        public ActionResult<IEnumerable<Dictionary<string, string>>> GetAllProjectFromMonacoHub();
        public IActionResult CheckRoom(string room);
        public IActionResult GetUsers();
        public IActionResult DisconnectUser(string username);

    }
}
