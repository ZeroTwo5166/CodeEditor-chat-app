using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace backend.Controllers
{
    [Route("api/code")]
    [ApiController]
    public class CodeController : Controller
    {
        private readonly IConfiguration _configuration;

        public CodeController(IConfiguration configuration)
        {
                _configuration = configuration;
        }


        [HttpPost("save")] //Dont need
        public async Task<IActionResult> SaveCode([FromBody] SqlModel sqlCommand)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Adjust the SQL query based on your database schema
                    string query = "  insert into CodeSnippets values (@Title, @Code);";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", sqlCommand.Title);
                        command.Parameters.AddWithValue("@Code", sqlCommand.Code);
                        await command.ExecuteNonQueryAsync();
                    }
                }

                return Ok(new { Success = true, Message = "Code saved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error saving code", Error = ex.Message });
            }
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertCode([FromBody] SqlModel payload)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Adjust the SQL query based on your database schema
                    string query = "  insert into CodeSnippets values (@Title, @Code);";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", payload.Title);
                        command.Parameters.AddWithValue("@Code", payload.Code);
                        await command.ExecuteNonQueryAsync();
                    }
                }

                return Ok(new { Success = true, Message = "Code inserted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error inserting code", Error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCode([FromBody] SqlModel payload)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Adjust the SQL query based on your database schema
                    string query = "UPDATE CodeSnippets SET CSharpCode = @Code WHERE Title = @Title";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", payload.Title);
                        command.Parameters.AddWithValue("@Code", payload.Code);
                        await command.ExecuteNonQueryAsync();
                    }
                }

                return Ok(new { Success = true, Message = "Code updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error updating code", Error = ex.Message });
            }
        }
    }
}
