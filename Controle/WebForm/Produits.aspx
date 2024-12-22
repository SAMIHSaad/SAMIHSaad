<%@ Page Title="Produits" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Produits.aspx.cs" Inherits="Controle.WebForm.Produits" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Produits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>Liste des Produits</h2>
        <asp:GridView ID="GridViewProduits" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
            <Columns>
                <asp:BoundField DataField="Nom" HeaderText="Nom du Produit" SortExpression="Nom" />
                <asp:BoundField DataField="Prix" HeaderText="Prix (MAD)" SortExpression="Prix" />
                <asp:BoundField DataField="Quantite" HeaderText="Quantité" SortExpression="Quantite" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="BtnSupprimer" runat="server" Text="Supprimer" CssClass="btn btn-danger" CommandArgument='<%# Eval("ProduitId") %>' OnCommand="BtnSupprimer_Command" />
                        <asp:Button ID="BtnModifier" runat="server" Text="Modifier" CssClass="btn btn-warning" CommandArgument='<%# Eval("ProduitId") %>' OnCommand="BtnModifier_Command" />

                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <h3>Ajouter ou Modifier un Produit</h3>
        <div class="form-group">
            <asp:TextBox ID="TxtNom" runat="server" CssClass="form-control" Placeholder="Nom du produit" />
        </div>
        <div class="form-group">
            <asp:TextBox ID="TxtPrix" runat="server" CssClass="form-control" Placeholder="Prix" TextMode="Number" />
        </div>
        <div class="form-group">
            <asp:TextBox ID="TxtQuantite" runat="server" CssClass="form-control" Placeholder="Quantité" TextMode="Number" />
        </div>
        <asp:HiddenField ID="HiddenProduitId" runat="server" />
        <asp:Button ID="BtnAjouter" runat="server" Text="Ajouter" CssClass="btn btn-primary" OnClick="BtnAjouter_Click" />
        <asp:Button ID="BtnModifier" runat="server" Text="Modifier" CssClass="btn btn-warning" OnClick="BtnModifier_Click" />
    </div>
</asp:Content>