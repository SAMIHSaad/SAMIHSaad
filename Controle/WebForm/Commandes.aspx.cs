using Controle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System.ComponentModel;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Lifetime;
using System.Security.Cryptography;
namespace Controle.WebForm
{
    public partial class Commandes : System.Web.UI.Page
    {
        private readonly AppDbContext _context = new AppDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ChargerCommandes();
                    ChargerProduits();
                }
            }
            catch (Exception ex)
            {
                // Log the error or display a user-friendly message
                Response.Write("<script>alert('Erreur lors du chargement des commandes : " + ex.Message + "');</script>");
            }
        }

        private void ChargerCommandes()
        {
            try
            {
                var commandes = _context.Commandes
                    .Include(c => c.Produits) // Ensure related products are loaded
                    .Select(c => new
                    {
                        c.Client,
                        c.DateCommande,
                        c.PrixTotal,
                        Produits = c.Produits.Select(p => new { p.Nom, p.Quantite }).ToList()
                    })
                    .OrderByDescending(c => c.DateCommande)
                    .ToList();

                // Log the count of commandes
                Response.Write("<script>console.log('Nombre de commandes : " + commandes.Count + "');</script>");

                if (commandes.Count == 0)
                {
                    Response.Write("<script>alert('Aucune commande trouvée.');</script>");
                }

                GridViewCommandes.DataSource = commandes;
                GridViewCommandes.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Erreur lors du chargement des commandes : " + ex.Message + "');</script>");
                if (ex.InnerException != null)
                {
                    Response.Write("<script>alert('Détails de l'erreur : " + ex.InnerException.Message + "');</script>");
                }
            }
        }
        private void ChargerProduits()
        {
            try
            {
                if (RepeaterProduits != null)
                {
                    var produits = _context.Produits.ToList();
                    if (produits.Count == 0)
                    {
                        Response.Write("<script>alert('Aucun produit trouvé.');</script>");
                    }
                    RepeaterProduits.DataSource = produits;
                    RepeaterProduits.DataBind();
                }
                else
                {
                    Response.Write("<script>alert('Erreur : RepeaterProduits n'est pas initialisé.');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Erreur lors du chargement des produits : " + ex.Message + "');</script>");
            }
        }

        protected void BtnAjouterCommande_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedProducts = RepeaterProduits.Items.Cast<RepeaterItem>()
                                            .Where(i => ((CheckBox)i.FindControl("CheckBoxProduit")).Checked)
                                            .Select(i =>
                                            {
                                                var hiddenField = i.FindControl("HiddenProduitId") as HiddenField;
                                                var textBox = i.FindControl("TxtQuantiteCommande") as TextBox;

                                                if (hiddenField == null || textBox == null)
                                                {
                                                    return null;
                                                }

                                                return new
                                                {
                                                    ProduitId = int.Parse(hiddenField.Value),
                                                    Quantite = int.Parse(textBox.Text)
                                                };
                                            })
                                            .Where(p => p != null)
                                            .ToList();

                decimal prixTotal = 0;
                List<Produit> produitsCommandes = new List<Produit>();

                foreach (var item in selectedProducts)
                {
                    var produit = _context.Produits.Find(item.ProduitId);
                    if (produit != null && produit.Quantite >= item.Quantite)
                    {
                        produit.Quantite -= item.Quantite; // Decrease quantity in the database
                        prixTotal += produit.Prix * item.Quantite;
                        _context.Entry(produit).State = EntityState.Modified;

                        produitsCommandes.Add(new Produit
                        {
                            ProduitId = produit.ProduitId,
                            Nom = produit.Nom,
                            Prix = produit.Prix,
                            Quantite = item.Quantite
                        });
                    }
                    else
                    {
                        Response.Write("<script>alert('Quantité insuffisante pour le produit " + produit.Nom + "');</script>");
                        return;
                    }
                }

                Commande commande = new Commande
                {
                    Client = TxtClient.Text,
                    DateCommande = DateTime.Now,
                    Produits = produitsCommandes,
                    PrixTotal = prixTotal
                };

                _context.Commandes.Add(commande);

                // Log the state of the context before saving
                var entries = _context.ChangeTracker.Entries();
                foreach (var entry in entries)
                {
                    Response.Write($"<script>console.log('Entity: {entry.Entity.GetType().Name}, State: {entry.State}');</script>");
                }

                _context.SaveChanges(); // Save all changes, including product updates and the new order

                ChargerCommandes(); // Refresh the order list
                ChargerProduits(); // Refresh the product list to reflect updated quantities

                LblPrixTotal.Text = "Prix Total: " + prixTotal.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("fr-MA"));
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Erreur lors de l'ajout de la commande : " + ex.Message + "');</script>");
                if (ex.InnerException != null)
                {
                    Response.Write("<script>alert('Détails de l'erreur : " + ex.InnerException.Message + "');</script>");
                }
            }
        }

        protected void CheckBoxProduit_CheckedChanged(object sender, EventArgs e)
        {
            CalculerPrixTotal();
        }

        protected void TxtQuantiteCommande_TextChanged(object sender, EventArgs e)
        {
            CalculerPrixTotal();
        }

        private void CalculerPrixTotal()
        {
            decimal prixTotal = 0;

            foreach (RepeaterItem item in RepeaterProduits.Items)
            {
                var checkBox = item.FindControl("CheckBoxProduit") as CheckBox;
                var textBox = item.FindControl("TxtQuantiteCommande") as TextBox;
                var hiddenField = item.FindControl("HiddenProduitId") as HiddenField;

                if (checkBox != null && checkBox.Checked && textBox != null && hiddenField != null)
                {
                    int produitId = int.Parse(hiddenField.Value);
                    var produit = _context.Produits.Find(produitId);

                    if (produit != null && int.TryParse(textBox.Text, out int quantite))
                    {
                        prixTotal += produit.Prix * quantite;
                    }
                }
            }

            LblPrixTotal.Text = "Prix Total: " + prixTotal.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("fr-MA"));
        }

        protected void RepeaterProduits_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var checkBox = e.Item.FindControl("CheckBoxProduit") as CheckBox;
                var textBox = e.Item.FindControl("TxtQuantiteCommande") as TextBox;

                if (checkBox != null)
                {
                    checkBox.CheckedChanged += CheckBoxProduit_CheckedChanged;
                }

                if (textBox != null)
                {
                    textBox.TextChanged += TxtQuantiteCommande_TextChanged;
                }
            }
        }

        protected void GridViewCommandes_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle selection change if needed
        }
    }
}