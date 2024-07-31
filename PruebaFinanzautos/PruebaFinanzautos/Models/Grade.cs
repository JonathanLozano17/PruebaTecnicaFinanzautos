using System.ComponentModel.DataAnnotations;

namespace PruebaFinanzautos.Models
{
    public class Grade
    {
        [Key]
        public int GradeId { get; set; }
        public int StudentsID { get; set; }
        public int CourseID { get; set; }
        public string GradeValue { get; set; }

        public Student Students { get; set; }
        public Course Course { get; set; }
    }
}
