using backend.Models;
using backend.ModelsDTO;

namespace backend.Interfaces
{
    public interface IUserRepository
    {
        Task<string> Register(User user);
        Task<string> Login(LoginDTO user);
        Task<User> GetIdByUsername(string username);
        Task<User> GetUserById(Guid id);
        Task<string> GetProfilePic(RetrieveImageDTO userImage);
        Task<byte[]> ConvertIFormFileToByteArray(IFormFile file);

        Task<byte[]> ConvertIFormFileToByteArrayInternal(IFormFile file);

    }
}
