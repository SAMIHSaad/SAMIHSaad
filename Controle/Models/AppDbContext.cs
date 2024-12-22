using System.Configuration;
using System.Data.Entity;

namespace Controle.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Commande> Commandes { get; set; }

        public AppDbContext() : base(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
        {
        }

    }
}
