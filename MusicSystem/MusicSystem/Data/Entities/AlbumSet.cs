using Mono.TextTemplating;
using System.ComponentModel.DataAnnotations;

namespace MusicSystem.Data.Entities
{
    public class AlbumSet
    {
        public int Id { get; set; }

        [Display(Name = "Album"),]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        public ICollection<SongSet> SongSets { get; set; }

        [Display(Name = "Canciones"),]
        public int SongNumber => SongSets == null ? 0 : SongSets.Count;
        public ICollection<PurchaseDetail>  PurchaseDetails { get; set; }

        [Display(Name = "Comprado"),]
        public int PurchaseNumber => PurchaseDetails == null ? 0 : PurchaseDetails.Count;
    }
}
