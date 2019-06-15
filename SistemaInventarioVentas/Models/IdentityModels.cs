using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SistemaInventarioVentas.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class SistemaInventarioDBContext : IdentityDbContext<ApplicationUser>
    {
        public SistemaInventarioDBContext()
            : base("SistemaInventario", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Producto>()
                .HasKey(p => p.ProductoID)
                .ToTable("Productos");

            modelBuilder
                .Entity<Categoria>()
                .HasKey(c => c.CategoriaID)
                .ToTable("Categorias");

            modelBuilder
                .Entity<Almacen>()
                .HasKey(a => a.AlmacenID)
                .ToTable("Almacen");

            modelBuilder
                .Entity<Venta>()
                .HasKey(v=> v.VentaID)
                .ToTable("Ventas");

            modelBuilder
                .Entity<Cliente>()
                .HasKey(c => c.ClienteID)
                .ToTable("Clientes");

            modelBuilder
                .Entity<DetalleVenta>()
                .HasKey(d => new { d.ProductoID, d.VentaID });

            modelBuilder
                .Entity<DetalleVenta>()
                .Property(d => d.ProductoID)
                .HasColumnName("ProductoID")
                .HasColumnOrder(1);

            modelBuilder
                .Entity<DetalleVenta>()
                .Property(d => d.VentaID)
                .HasColumnName("VentaID")
                .HasColumnOrder(0);

        }

        public static SistemaInventarioDBContext Create()
        {
            return new SistemaInventarioDBContext();
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Almacen> Almacenes { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }

    }
}