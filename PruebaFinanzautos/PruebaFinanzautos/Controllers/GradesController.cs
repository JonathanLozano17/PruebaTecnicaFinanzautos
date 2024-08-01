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
    public class GradesController : ControllerBase
    {
        private readonly string _connectionString;

        public GradesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }


        // GET: api/grades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grade>>> Get()
        {
            var grades = new List<Grade>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_GetAllGrades", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                grades.Add(new Grade
                                {
                                    GradeId = reader.GetInt32(0),
                                    StudentsID = reader.GetInt32(1),
                                    CourseID = reader.GetInt32(2),
                                    GradeValue = reader.GetString(3)
                                });
                            }
                        }
                    }
                }
                return Ok(grades);
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


        // GET: api/grades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Grade>> Get(int id)
        {
            Grade grade = null;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_GetGrade", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@GradeID", id);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                grade = new Grade
                                {
                                    GradeId = reader.GetInt32(0),
                                    StudentsID = reader.GetInt32(1),
                                    CourseID = reader.GetInt32(2),
                                    GradeValue = reader.GetString(3)
                                };
                            }
                        }
                    }
                }
                if (grade == null)
                {
                    return NotFound();
                }
                return Ok(grade);
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


        // POST: api/grades
        [HttpPost]
        public async Task<ActionResult<Grade>> Post([FromBody] GradeDto gradeDto)
        {

            if (gradeDto == null || string.IsNullOrEmpty(gradeDto.GradeValue))
            {
                return BadRequest("Invalid grade data");
            }

            Grade grade = new()
            {
                GradeId = gradeDto.GradeId,
                StudentsID = gradeDto.StudentsID,
                CourseID = gradeDto.CourseID,
                GradeValue = gradeDto.GradeValue
            };

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_InsertGrade", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentsId", gradeDto.StudentsID);
                        command.Parameters.AddWithValue("@CoursesID", gradeDto.CourseID);
                        command.Parameters.AddWithValue("@Grade", gradeDto.GradeValue);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                // Return a response indicating the location of the newly created resource
                return CreatedAtAction(nameof(Get), new { id = gradeDto.GradeId }, gradeDto);
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

        // PUT: api/grades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] GradeDto gradeDto)
        {
            if (id != gradeDto.GradeId)
            {
                return BadRequest("Grade ID mismatch");
            }

            Grade grade = new()
            {
                GradeId = gradeDto.GradeId,
                StudentsID = gradeDto.StudentsID,
                CourseID = gradeDto.CourseID,
                GradeValue = gradeDto.GradeValue
            };

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_UpdateGrade", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@GradeID", id);
                        command.Parameters.AddWithValue("@StudentsId", grade.StudentsID);
                        command.Parameters.AddWithValue("@CoursesID", grade.CourseID);
                        command.Parameters.AddWithValue("@Grade", grade.GradeValue);
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

        // DELETE: api/grades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_DeleteGrade", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@GradeID", id);
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
