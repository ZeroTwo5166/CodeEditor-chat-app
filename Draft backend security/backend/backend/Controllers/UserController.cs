using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller, IUserController
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto user)
        {
            
            // Check if the email already exists
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return BadRequest("Email already exists");
            }

            // Check if the username already exists
            else if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                return BadRequest("Username already exists");
            }

            byte[] passwordHash, passwordKey;

            using (var hmc = new HMACSHA512())
            {
                passwordKey = hmc.Key;
                passwordHash = hmc.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
            }

            User userToSend = new User();
            userToSend.Username = user.Username;
            userToSend.Email = user.Email;
            userToSend.Password = passwordHash;
            userToSend.PasswordKey = passwordKey;

            //if user doesnt choose a image, random image will be applied
            if (user.ProfilePic == null)
            {
                string filePath = @"E:\Presentation\Draft backend security\backend\backend\Assets\default-pp.png";

                try
                {   //no need to call dispose since it will be automatically be disposed as I have implemented "using" statement 
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        // Create a byte array to hold the contents of the file
                        byte[] fileBytes = new byte[fs.Length];

                        // Read the file into the byte array
                        fs.Read(fileBytes, 0, fileBytes.Length);

                        userToSend.ProfilePic = fileBytes;
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            } else
            {
                userToSend.ProfilePic = user.ProfilePic;
            }

            //Add the user to the database
            _context.Users.Add(userToSend);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, user);
            return Ok(new { Username = user.Username, Message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login user)
        {


            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser == null)
            {
                return BadRequest("Email is not singed up!");
            }

            // Check if the entered password is correct
            //if (existingUser.Password != user.Password)
            //{
            //    return BadRequest("Wrong password!!!");
            //}

            // Perform additional login logic if needed
            //Verify password
            if (!MatchPasswordHash(user.Password, existingUser.Password, existingUser.PasswordKey))
                return BadRequest("Wrong password!!");


            return Ok(new { 
                Message = "Login Successful ",
                Username = existingUser.Username,
                Email = existingUser.Email,
                ProfilePic = existingUser.ProfilePic
            });
        }
         
        [HttpPost("retrieve")]
        public async Task<ActionResult> GetProfilePic(RetrieveImage userImage)
        {
            var user =  _context.Users
                .Where(u => u.Username == userImage.Username)
                .Select(u => new { u.Id, u.ProfilePic})
                .FirstOrDefault();

            if(user != null)
            {
                if(user.ProfilePic != null)
                {
                    string imageBase64Data = Convert.ToBase64String(user.ProfilePic);
                    string imageDataURL = string.Format("data:image/jpg;base64,{0}",
                    imageBase64Data);

                    return Ok(new { Id = user.Id, Data = imageDataURL });
                }
                else
                {
                    return BadRequest("Profilepic not found");
                }
            }
            else
            {
                return BadRequest("User not found");
            } 
        }


        //Using a model type of id because normal types didnt work
        [HttpGet("getid")]
        public async Task<IActionResult> GetId([FromBody] Getid id)
        {
            try
            {
                var userExists = await _context.Users.Where(x => x.Id == id.Id).FirstOrDefaultAsync(); 
                if(userExists != null)
                {
                    return Ok( new {user = userExists});
                }
                else
                {
                    return NotFound("User not found");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("getidbyusername/{username}")]
        public async Task<IActionResult> GetIdByUsername([FromRoute] string username)
        {
            try
            {
                var user = await _context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
                if (user != null)
                {
                    return Ok(new { userId = user.Id });
                }
                else
                {
                    return NotFound("User not found");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("byteConverter")]
        //api function to convert IFormFile to byte[] using local function
        public IActionResult ConvertIFormFileToByteArray([FromForm] IFormFile file)
        {
            try
            {
                byte[] byteArray = ConvertIFormFileToByteArrayInternal(file);
                return Ok(byteArray);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error converting file to byte array: {ex.Message}");
            }
        }
        
        //local function to convert IFormFile to byte[]
        public byte[] ConvertIFormFileToByteArrayInternal(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Copy the content of the file to the memory stream
                file.CopyTo(memoryStream);

                // Convert the memory stream to a byte array
                byte[] byteArray =  memoryStream.ToArray();

                return byteArray;
            }
        }

        private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
        {
            using (var hmc = new HMACSHA512(passwordKey))
            {
                var passwordHash = hmc.ComputeHash(Encoding.UTF8.GetBytes(passwordText));

                for(int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != password[i])
                        return false;
                }

                return true;
            }
        }
    }
}
