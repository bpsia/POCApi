namespace POCService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCommit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerNumber = c.Int(nullable: false),
                        CustomerName = c.String(),
                        CustomerType = c.String(),
                        TotalAmountOfSales = c.Double(nullable: false),
                        Timestamp = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SalesCustomers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerNumber = c.Int(nullable: false),
                        CustomerName = c.String(),
                        CustomerType = c.String(),
                        TotalAmountOfSales = c.Double(nullable: false),
                        Timestamp = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        PasswordSalt = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.SalesCustomers");
            DropTable("dbo.Customers");
        }
    }
}
