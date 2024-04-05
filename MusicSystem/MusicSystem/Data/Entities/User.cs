using Microsoft.AspNetCore.Identity;
using MusicSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace MusicSystem.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Identificacion")]
        [MaxLength(10, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; }

        [Display(Name = "Tipo de usuario")]
        public UserType UserType { get; set; }
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
        [Display(Name = "Compras"),]
        public int PurchaseNumber => PurchaseDetails == null ? 0 : PurchaseDetails.Count;

    }
}
