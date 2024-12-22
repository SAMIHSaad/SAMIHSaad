using System.ComponentModel.DataAnnotations;

namespace Controle.Models
{
    public class Produit
    {
        [Key]
        public int ProduitId { get; set; }

        [Required]
    
        public string Nom { get; set; }

        [Required]
        
        public decimal Prix { get; set; }

        [Required]
        public int Quantite { get; set; }


        public int? CommandeId { get; set; }
        public virtual Commande Commande { get; set; }


    }
}