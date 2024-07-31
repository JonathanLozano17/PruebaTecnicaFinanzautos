using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace PruebaFinanzautos.Models
{
    public class Student
    {
        [Key]
        public int StudentsId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public ICollection<Grade> Grades { get; set; }

    }
}
