<%@ Page Title="Commandes" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Commandes.aspx.cs" Inherits="Controle.WebForm.Commandes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Commandes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>Liste des Commandes</h2>
        <asp:GridView ID="GridViewCommandes" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
            <Columns>
                <asp:BoundField DataField="Client" HeaderText="Client" />
                <asp:BoundField DataField="DateCommande" HeaderText="Date de Commande" DataFormatString="{0:dd/MM/yyyy}" />

                <asp:TemplateField HeaderText="Produits Commandés">
                    <ItemTemplate>
                        <asp:Repeater ID="RepeaterProduitsCommandes" runat="server" DataSource='<%# Eval("Produits") %>'>
                            <ItemTemplate>
                                <%# Eval("Nom") %> (Quantité: <%# Eval("Quantite") %>)<br />
                            </ItemTemplate>
                        </asp:Repeater>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Prix Total (MAD)">
                    <ItemTemplate>
                        <%# Eval("PrixTotal", "{0:C}") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <h3>Créer une Commande</h3>
        <div class="form-group">
            <asp:TextBox ID="TxtClient" runat="server" CssClass="form-control" Placeholder="Nom du Client" />
        </div>
        <div class="form-group">
            <asp:Repeater ID="RepeaterProduits" runat="server" OnItemDataBound="RepeaterProduits_ItemDataBound">
                <HeaderTemplate>
                    <table class="table table-bordered">
                        <tr>
                            <th>Sélectionner</th>
                            <th>Nom du Produit</th>
                            <th>Prix (MAD)</th>
                            <th>Quantité Disponible</th>
                            <th>Quantité Commandée</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><asp:CheckBox ID="CheckBoxProduit" runat="server" OnCheckedChanged="CheckBoxProduit_CheckedChanged" AutoPostBack="true" /></td>
                        <td><%# Eval("Nom") %></td>
                        <td><%# Eval("Prix") %></td>
                        <td><%# Eval("Quantite") %></td>
                        <td><asp:TextBox ID="TxtQuantiteCommande" runat="server" CssClass="form-control" TextMode="Number" OnTextChanged="TxtQuantiteCommande_TextChanged" AutoPostBack="true" /></td>
                        <td><asp:HiddenField ID="HiddenProduitId" runat="server" Value='<%# Eval("ProduitId") %>' /></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <asp:Button ID="BtnAjouterCommande" runat="server" Text="Ajouter Commande" CssClass="btn btn-primary" OnClick="BtnAjouterCommande_Click" />
        <div class="form-group">
            <asp:Label ID="LblPrixTotal" runat="server" CssClass="form-control" />
        </div>
    </div>
</asp:Content>
