using System.ComponentModel.DataAnnotations;

namespace PruebaFinanzautos.DTOs
{
    

    public class StudentDto
    {
        [Key]
        public int StudentsId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int idGrade { get; set; }


    }
}
