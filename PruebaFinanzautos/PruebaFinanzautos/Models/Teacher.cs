using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace PruebaFinanzautos.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }

        public ICollection<Course> Courses { get; set; }

    }
}
