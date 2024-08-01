using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PruebaFinanzautos.DTOs;
using PruebaFinanzautos.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaFinanzautos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                                    Adress = reader.GetString(6)
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
                                    Adress = reader.GetString(6)
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
        public async Task<ActionResult<Teacher>> Post([FromBody] TeacherDto teacherDto)
        {

            Teacher teacher = new()
            {
                TeacherId = teacherDto.TeacherId,
                FirstName = teacherDto.FirstName,
                LastName = teacherDto.LastName,
                Email = teacherDto.Email,
                Phone = teacherDto.Phone,
                Department = teacherDto.Department,
                Adress = teacherDto.Adress
            };

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
                        command.Parameters.AddWithValue("@Adress", teacher.Adress);

                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            // Ideally, you would want to retrieve the new ID if possible, depending on how your procedure is set up.
                            // For now, we just return the created teacher without an ID.
                            return CreatedAtAction(nameof(Get), new { id = teacher.TeacherId }, teacher);
                        }
                        else
                        {
                            return StatusCode(500, "Error creating teacher");
                        }
                    }
                }
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
        public async Task<IActionResult> Put(int id, [FromBody] TeacherDto teacherDto)
        {

            Teacher teacher = new()
            {
                TeacherId = teacherDto.TeacherId,
                FirstName = teacherDto.FirstName,
                LastName = teacherDto.LastName,
                Email = teacherDto.Email,
                Phone = teacherDto.Phone,
                Department = teacherDto.Department,
                Adress = teacherDto.Adress
            };

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
                        command.Parameters.AddWithValue("@Adress", teacher.Adress);
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
