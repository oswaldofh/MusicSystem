using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicSystem.Data.Entities;

namespace MusicSystem.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        //SE INYECTA EL CONTEXTO DE LA BASE DE DATOS
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //SE AGREGA LA ENTIDAD QUE SE VA A CREAR EN LA VASE DE DATOS
        public DbSet<AlbumSet>  AlbumSets { get; set; }
        public DbSet<SongSet> SongSets { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //SE AGREGA UN INDEX DEL NOMBRE PARA QUE NO SE REPITA UN NOMBRE DEL ALBUN
            modelBuilder.Entity<AlbumSet>().HasIndex(a => a.Name).IsUnique(); 
            modelBuilder.Entity<SongSet>().HasIndex("Name", "AlbumSetId").IsUnique();//indice compuesto, no se repite el nombre de la cancion en el albun
           
        }
    }
}
