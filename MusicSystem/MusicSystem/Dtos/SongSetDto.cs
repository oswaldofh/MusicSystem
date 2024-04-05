using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MusicSystem.Dtos
{
    public class SongSetDto
    {
        public int Id { get; set; }

        [Display(Name = "Canción"),]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        [Display(Name = "Album")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un album")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int AlbumSetId { get; set; }
        public IEnumerable<SelectListItem> Albumes { get; set; }
    }
}
