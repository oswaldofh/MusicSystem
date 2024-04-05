using System.ComponentModel.DataAnnotations;

namespace MusicSystem.Data.Entities
{
    public class PurchaseDetail
    {
        public int Id { get; set; }
        public User User { get; set; }
        public AlbumSet AlbumSet { get; set; }
        public double Total {  get; set; }
    }
}
