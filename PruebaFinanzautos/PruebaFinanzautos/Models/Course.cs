using System.ComponentModel.DataAnnotations;

namespace PruebaFinanzautos.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CorseName { get; set; }
        public int Credits { get; set; }
        public int? TeacherId { get; set; }

        public Teacher Teacher { get; set; }
        public ICollection<Grade> Grades { get; set; }
    }
}
