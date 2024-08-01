using System.ComponentModel.DataAnnotations;

namespace PruebaFinanzautos.DTOs
{
    public class CourseDto

    {
        [Key]
        public int CourseId { get; set; }
        public string CorseName { get; set; }
        public int Credits { get; set; }
        public int? TeacherId { get; set; }

    }
}
