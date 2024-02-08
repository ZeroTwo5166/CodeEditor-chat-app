using backend.Interfaces;
using backend.Models;
using backend.ModelsDTO;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        //Dependency injection
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //Using a generic delegate
        private delegate Task<string> UserOperation<T>(T user);


        // Implement a method that uses the delegate
        private async Task<string> ExecuteUserOperationAsync<T>(T user, UserOperation<T> operation)
        {
            try
            {
                // Execute the provided operation
                return await operation(user);
            }
            catch (Exception)
            {
                // Handle exceptions here
                return "An error occurred during user operation.";
            }
        }

        //login method --> Uses lambda expression to execute operation with the provided user data 
        public async Task<string> Login(LoginDTO user)
        {
            return await ExecuteUserOperationAsync(user, async (u) => {

                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

                if (existingUser == null)
                {
                    return "User not found";
                }

                // Check if the entered password is correct
                if (existingUser.Password != user.Password)
                {
                    return "Wrong password";
                }
                await Task.Delay(100); // Simulating login process

                return "Success";
            });   

        }

        //Register Method
        public async Task<string> Register(User user)
        {
            // Using the generic delegate 
            return await ExecuteUserOperationAsync(user, async (u) => {

                // Check if the email already exists
                if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                {
                    return "Email already exists";
                }

                // Check if the username already exists
                else if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                {
                    return "Username already exists";
                }

                //if user doesnt choose a image, random image will be applied
                if (user.ProfilePic == null)
                {
                    string filePath = @"C:\Users\subar\Desktop\finalProject\backend\backend\Assets\default-pp.png";

                    try
                    {   //no need to call dispose since it will be automatically be disposed as I have implemented "using" statement 
                        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            // Create a byte array to hold the contents of the file
                            byte[] fileBytes = new byte[fs.Length];

                            // Read the file into the byte array
                            fs.Read(fileBytes, 0, fileBytes.Length);

                            user.ProfilePic = fileBytes;
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }

                }

                //Add the user to the database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await Task.Delay(100); // Simulating registration process
                //return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, user);
                return "Registration successful";

            });


        }

        public async Task<User> GetIdByUsername(string username)
        {
            var user = await _context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
            return user; 
        }

        public async Task<string> GetProfilePic(RetrieveImageDTO userImage)
        {
            // Example usage of the generic delegate
            return await ExecuteUserOperationAsync(userImage, async (image) => {
                var user = _context.Users
                .Where(u => u.Username == userImage.Username)
                .Select(u => new { u.Id, u.ProfilePic })
                .FirstOrDefault();

                if (user != null)
                {
                    if (user.ProfilePic != null)
                    {
                        string imageBase64Data = Convert.ToBase64String(user.ProfilePic);
                        string imageDataURL = string.Format("data:image/jpg;base64,{0}",
                        imageBase64Data);

                        return imageDataURL;
                    }
                    else
                    {
                        return "Profilepic not found";
                    }
                }
                else
                {
                    return "User not found";
                }
            });    
        }

        public async Task<User> GetUserById(Guid id)
        {
            var userExists = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            return userExists;
        }
        //Uses the internal function to solve for it
        public Task<byte[]> ConvertIFormFileToByteArray(IFormFile file)
        {
              return ConvertIFormFileToByteArrayInternal(file);
        }

        public Task<byte[]> ConvertIFormFileToByteArrayInternal(IFormFile file)
        {

            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Copy the content of the file to the memory stream
                file.CopyTo(memoryStream);

                // Convert the memory stream to a byte array
                byte[] byteArray = memoryStream.ToArray();

                return Task.FromResult(byteArray);
            }
        }

    }
}
