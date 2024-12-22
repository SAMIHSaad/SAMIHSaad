namespace Controle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtotal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Commandes",
                c => new
                    {
                        CommandeId = c.Int(nullable: false, identity: true),
                        DateCommande = c.DateTime(nullable: false),
                        Client = c.String(nullable: false, maxLength: 100),
                        PrixTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CommandeId);
            
            CreateTable(
                "dbo.Produits",
                c => new
                    {
                        ProduitId = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false),
                        Prix = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantite = c.Int(nullable: false),
                        CommandeId = c.Int(),
                    })
                .PrimaryKey(t => t.ProduitId)
                .ForeignKey("dbo.Commandes", t => t.CommandeId)
                .Index(t => t.CommandeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Produits", "CommandeId", "dbo.Commandes");
            DropIndex("dbo.Produits", new[] { "CommandeId" });
            DropTable("dbo.Produits");
            DropTable("dbo.Commandes");
        }
    }
}
