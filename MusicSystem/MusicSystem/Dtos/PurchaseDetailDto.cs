using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MusicSystem.Dtos
{
    public class PurchaseDetailDto
    {
        public int Id { get; set; }

        [Display(Name = "Total"),]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public double Total { get; set; }

        [Display(Name = "Album")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un album")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int AlbumSetId { get; set; }
        public IEnumerable<SelectListItem> Albumes { get; set; }
    }
}
