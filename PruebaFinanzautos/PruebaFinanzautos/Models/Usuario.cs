using System.ComponentModel.DataAnnotations;

namespace PruebaFinanzautos.Models
{
    public class Usuario
    {
         [Key]
        public int UsuarioId { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string ClaveHash { get; set; }

    }
}
