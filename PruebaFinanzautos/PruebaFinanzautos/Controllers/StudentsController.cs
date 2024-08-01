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
    public class StudentsController : ControllerBase
    {
        private readonly string _connectionString;

        public StudentsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }


        // GET: api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> Get()
        {
            var students = new List<Student>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_GetAllStudents", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            students.Add(new Student
                            {
                                StudentsId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                DateOfBirth = reader.GetDateTime(3),
                                Email = reader.GetString(4),
                                Phone = reader.GetString(5),
                                Address = reader.GetString(6)
                            });
                        }
                    }
                }
            }
            return students;
        }


        // GET: api/students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> Get(int id)
        {
            Student student = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_GetStudent", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentsId", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            student = new Student
                            {
                                StudentsId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                DateOfBirth = reader.GetDateTime(3),
                                Email = reader.GetString(4),
                                Phone = reader.GetString(5),
                                Address = reader.GetString(6)
                            };
                        }
                    }
                }
            }
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }


        // POST: api/students
        [HttpPost]
        public async Task<ActionResult<Student>> Post([FromBody] StudentDto studentDto)
        {


            Student student = new()
            {
                StudentsId = studentDto.StudentsId,
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                DateOfBirth = studentDto.DateOfBirth,
                Email = studentDto.Email,
                Phone = studentDto.Phone,
                Address = studentDto.Address,
                idGrade = studentDto.idGrade
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_InsertStudent", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                    command.Parameters.AddWithValue("@Email", student.Email);
                    command.Parameters.AddWithValue("@Phone", student.Phone);
                    command.Parameters.AddWithValue("@Adress", student.Address);
                    await command.ExecuteNonQueryAsync();
                }
            }
            return CreatedAtAction(nameof(Get), new { id = student.StudentsId }, student);
        }

        // PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] StudentDto studentDto)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Student student = new()
            {
                StudentsId = studentDto.StudentsId,
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                DateOfBirth = studentDto.DateOfBirth,
                Email = studentDto.Email,
                Phone = studentDto.Phone,
                Address = studentDto.Address,
                idGrade = studentDto.idGrade
            };


            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_UpdateStudent", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentsId", id);
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                    command.Parameters.AddWithValue("@Email", student.Email);
                    command.Parameters.AddWithValue("@Phone", student.Phone);
                    command.Parameters.AddWithValue("@Adress", student.Address);
                    await command.ExecuteNonQueryAsync();
                }
            }
            return NoContent();
        }

        // DELETE: api/students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_DeleteStudent", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentsId", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
            return NoContent();
        }
    }
}
