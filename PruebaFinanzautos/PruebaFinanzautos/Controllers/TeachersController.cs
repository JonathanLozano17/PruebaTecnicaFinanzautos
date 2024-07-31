using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PruebaFinanzautos.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaFinanzautos.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class TeachersController : ControllerBase
    {
        private readonly string _connectionString;

        public TeachersController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // GET: api/teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> Get()
        {
            var teachers = new List<Teacher>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_GetAllTeachers", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                teachers.Add(new Teacher
                                {
                                    TeacherId = reader.GetInt32(0),
                                    FirstName = reader.GetString(1),
                                    LastName = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    Phone = reader.GetString(4),
                                    Department = reader.GetString(5),
                                    Address = reader.GetString(6)
                                });
                            }
                        }
                    }
                }
                return Ok(teachers);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/teachers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> Get(int id)
        {
            Teacher teacher = null;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_GetTeacher", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TeacherId", id);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                teacher = new Teacher
                                {
                                    TeacherId = reader.GetInt32(0),
                                    FirstName = reader.GetString(1),
                                    LastName = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    Phone = reader.GetString(4),
                                    Department = reader.GetString(5),
                                    Address = reader.GetString(6)
                                };
                            }
                        }
                    }
                }
                if (teacher == null)
                {
                    return NotFound();
                }
                return Ok(teacher);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/teachers
        [HttpPost]
        public async Task<ActionResult<Teacher>> Post([FromBody] Teacher teacher)
        {
            if (teacher == null || string.IsNullOrEmpty(teacher.FirstName) || string.IsNullOrEmpty(teacher.LastName))
            {
                return BadRequest("Invalid teacher data");
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_InsertTeacher", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@FirstName", teacher.FirstName);
                        command.Parameters.AddWithValue("@LastName", teacher.LastName);
                        command.Parameters.AddWithValue("@Email", teacher.Email);
                        command.Parameters.AddWithValue("@Phone", teacher.Phone);
                        command.Parameters.AddWithValue("@Department", teacher.Department);
                        command.Parameters.AddWithValue("@Adress", teacher.Address);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                // To properly return the newly created teacher with its ID, consider returning a response with the appropriate status and location.
                return CreatedAtAction(nameof(Get), new { id = teacher.TeacherId }, teacher);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/teachers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                return BadRequest("Teacher ID mismatch");
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_UpdateTeacher", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TeacherId", id);
                        command.Parameters.AddWithValue("@FirstName", teacher.FirstName);
                        command.Parameters.AddWithValue("@LastName", teacher.LastName);
                        command.Parameters.AddWithValue("@Email", teacher.Email);
                        command.Parameters.AddWithValue("@Phone", teacher.Phone);
                        command.Parameters.AddWithValue("@Department", teacher.Department);
                        command.Parameters.AddWithValue("@Adress", teacher.Address);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return NoContent();
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/teachers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_DeleteTeacher", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TeacherId", id);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return NoContent();
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
