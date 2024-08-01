using PruebaFinanzautos.Models;
using System.ComponentModel.DataAnnotations;

namespace PruebaFinanzautos.DTOs
{
    public class GradeDto
    {
        [Key]
        public int GradeId { get; set; }
        public int StudentsID { get; set; }
        public int CourseID { get; set; }
        public string GradeValue { get; set; }

    }
}
