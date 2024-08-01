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
    [Route("api/[Controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly string _connectionString;

        public CoursesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // GET: api/courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> Get()
        {
            var courses = new List<Course>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_GetAllCourses", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                courses.Add(new Course
                                {
                                    CourseId = reader.GetInt32(0),
                                    CorseName = reader.GetString(1),
                                    Credits = reader.GetInt32(2),
                                    TeacherId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3)
                                });
                            }
                        }
                    }
                }
                return Ok(courses);
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


        // GET: api/courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> Get(int id)
        {
            Course course = null;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_GetCourse", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CourseId", id);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                course = new Course
                                {
                                    CourseId = reader.GetInt32(0),
                                    CorseName = reader.GetString(1),
                                    Credits = reader.GetInt32(2),
                                    TeacherId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3)
                                };
                            }
                        }
                    }
                }
                if (course == null)
                {
                    return NotFound();
                }
                return Ok(course);
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


        // POST: api/courses
        [HttpPost]
        public async Task<ActionResult<Course>> Post([FromBody] CourseDto courseDto)
        {

            Course course = new Course()
            {
                CourseId = courseDto.CourseId,
                CorseName = courseDto.CorseName,
                Credits = courseDto.Credits,
                TeacherId = courseDto.TeacherId
            };

            if (course == null || string.IsNullOrEmpty(course.CorseName))
            {
                return BadRequest("Invalid course data");
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_InsertCourse", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CourseName", course.CorseName);
                        command.Parameters.AddWithValue("@Credits", course.Credits);
                        command.Parameters.AddWithValue("@TeacherId", (object)course.TeacherId ?? DBNull.Value);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                // Return a response indicating the location of the newly created resource
                return CreatedAtAction(nameof(Get), new { id = course.CourseId }, course);
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

        // PUT: api/courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CourseDto courseDto)
        {

            Course course = new Course()
            {
                CourseId = courseDto.CourseId,
                CorseName = courseDto.CorseName,
                Credits = courseDto.Credits,
                TeacherId = courseDto.TeacherId
            };

            if (id == null)
            {
                return BadRequest("ID no puede ser null");
            }

            

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_UpdateCourse", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CourseId", id);
                        command.Parameters.AddWithValue("@CourseName", course.CorseName);
                        command.Parameters.AddWithValue("@Credits", course.Credits);
                        command.Parameters.AddWithValue("@TeacherId", (object)course.TeacherId ?? DBNull.Value);
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


        // DELETE: api/courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_DeleteCourse", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CourseId", id);
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
