using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Controle.Models
{
    public class Commande
    {
        [Key]
        public int CommandeId { get; set; }

        [Required]
        public DateTime DateCommande { get; set; }

        [Required]
        [StringLength(100)]
        public string Client { get; set; }

        [Required]
        public decimal PrixTotal { get; set; }

        [Required]
        public virtual ICollection<Produit> Produits { get; set; }
    }
}