using System.ComponentModel.DataAnnotations;

namespace PruebaFinanzautos.DTOs
{
    public class TeacherDto
    {
        [Key]
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Adress { get; set; }
    }
}
