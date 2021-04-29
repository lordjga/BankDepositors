namespace BankDepositors.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountingForDeposits2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        PhoneNumber = c.String(),
                        Passport = c.String(),
                        City = c.String(),
                        Adress = c.String(),
                        DepositName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Deposits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Term = c.Int(nullable: false),
                        Percent = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DepositClients",
                c => new
                    {
                        Deposit_Id = c.Int(nullable: false),
                        Client_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Deposit_Id, t.Client_Id })
                .ForeignKey("dbo.Deposits", t => t.Deposit_Id, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.Client_Id, cascadeDelete: true)
                .Index(t => t.Deposit_Id)
                .Index(t => t.Client_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DepositClients", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.DepositClients", "Deposit_Id", "dbo.Deposits");
            DropIndex("dbo.DepositClients", new[] { "Client_Id" });
            DropIndex("dbo.DepositClients", new[] { "Deposit_Id" });
            DropTable("dbo.DepositClients");
            DropTable("dbo.Deposits");
            DropTable("dbo.Clients");
        }
    }
}
