<%@ Page Title="Accueil" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Acceuil.aspx.cs" Inherits="Controle.WebForm.Acceuil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Accueil
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>Bienvenue dans l'application de gestion des produits et commandes</h2>
        <p>Utilisez le menu pour naviguer entre les pages.</p>
    </div>
</asp:Content>
