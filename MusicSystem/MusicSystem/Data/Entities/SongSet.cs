using Mono.TextTemplating;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicSystem.Data.Entities
{
    public class SongSet
    {
        public int Id { get; set; }

        [Display(Name = "Canción"),]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        public AlbumSet AlbumSet { get; set; }
    }
}
