using Controle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Controle.WebForm
{
    public partial class Produits : System.Web.UI.Page
    {
        private readonly AppDbContext context = new AppDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ChargerProduits();
            }
        }

        private void ChargerProduits()
        {
            try
            {
                List<Produit> produits = GetAllProduits();
                if (produits.Count == 0)
                {
                    Response.Write("<script>alert('Aucun produit trouvé dans la base de données.');</script>");
                }

                GridViewProduits.DataSource = produits;
                GridViewProduits.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Erreur : " + ex.Message + "');</script>");
            }
        }

        private void AjouterProduit(string nom, decimal prix, int quantite)
        {
            try
            {
                Produit produit = new Produit
                {
                    Nom = nom.Trim(),
                    Prix = prix,
                    Quantite = quantite
                };

                context.Produits.Add(produit);
                context.SaveChanges();

                Response.Write("<script>alert('Produit ajouté avec succès.');</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Erreur lors de l\\'ajout du produit : " + ex.Message + "');</script>");
            }
        }

        private void SupprimerProduit(int produitId)
        {
            try
            {
                var produit = context.Produits.Find(produitId);
                if (produit != null)
                {
                    context.Produits.Remove(produit);
                    context.SaveChanges();

                    Response.Write("<script>alert('Produit supprimé avec succès.');</script>");
                }
                else
                {
                    Response.Write("<script>alert('Produit introuvable.');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Erreur lors de la suppression : " + ex.Message + "');</script>");
            }
        }

        private void ModifierProduit(int produitId, string nom, decimal prix, int quantite)
        {
            try
            {
                var produit = context.Produits.Find(produitId);
                if (produit != null)
                {
                    produit.Nom = nom.Trim();
                    produit.Prix = prix;
                    produit.Quantite = quantite;

                    context.SaveChanges();

                    Response.Write("<script>alert('Produit modifié avec succès.');</script>");
                }
                else
                {
                    Response.Write("<script>alert('Produit introuvable.');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Erreur lors de la modification : " + ex.Message + "');</script>");
            }
        }

        private List<Produit> GetAllProduits()
        {
            try
            {
                return context.Produits.ToList();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Erreur lors de la récupération des produits : " + ex.Message + "');</script>");
                return new List<Produit>();
            }
        }

        private Produit GetProduitById(int produitId)
        {
            try
            {
                return context.Produits.Find(produitId);
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Erreur lors de la récupération du produit : " + ex.Message + "');</script>");
                return null;
            }
        }

        protected void BtnAjouter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNom.Text))
            {
                Response.Write("<script>alert('Le nom du produit est obligatoire.');</script>");
                return;
            }

            if (!decimal.TryParse(TxtPrix.Text, out decimal prix) || prix <= 0)
            {
                Response.Write("<script>alert('Veuillez entrer un prix valide.');</script>");
                return;
            }

            if (!int.TryParse(TxtQuantite.Text, out int quantite) || quantite <= 0)
            {
                Response.Write("<script>alert('Veuillez entrer une quantité valide.');</script>");
                return;
            }

            AjouterProduit(TxtNom.Text, prix, quantite);
            ChargerProduits();

            TxtNom.Text = "";
            TxtPrix.Text = "";
            TxtQuantite.Text = "";
        }

        protected void BtnSupprimer_Command(object sender, CommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int produitId))
            {
                SupprimerProduit(produitId);
                ChargerProduits();
            }
            else
            {
                Response.Write("<script>alert('ID du produit invalide.');</script>");
            }
        }

        protected void BtnModifier_Command(object sender, CommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int produitId))
            {
                var produit = GetProduitById(produitId);
                if (produit != null)
                {
                    HiddenProduitId.Value = produit.ProduitId.ToString();
                    TxtNom.Text = produit.Nom;
                    TxtPrix.Text = produit.Prix.ToString();
                    TxtQuantite.Text = produit.Quantite.ToString();
                }
                else
                {
                    Response.Write("<script>alert('Produit introuvable.');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('ID du produit invalide.');</script>");
            }
        }

        protected void BtnModifier_Click(object sender, EventArgs e)
        {
            if (int.TryParse(HiddenProduitId.Value, out int produitId))
            {
                if (string.IsNullOrWhiteSpace(TxtNom.Text))
                {
                    Response.Write("<script>alert('Le nom du produit est obligatoire.');</script>");
                    return;
                }

                if (!decimal.TryParse(TxtPrix.Text, out decimal prix) || prix <= 0)
                {
                    Response.Write("<script>alert('Veuillez entrer un prix valide.');</script>");
                    return;
                }

                if (!int.TryParse(TxtQuantite.Text, out int quantite) || quantite < 0)
                {
                    Response.Write("<script>alert('Veuillez entrer une quantité valide.');</script>");
                    return;
                }

                ModifierProduit(produitId, TxtNom.Text, prix, quantite);
                ChargerProduits();

                TxtNom.Text = "";
                TxtPrix.Text = "";
                TxtQuantite.Text = "";
            }
            else
            {
                Response.Write("<script>alert('ID du produit invalide.');</script>");
            }
        }
    }
}