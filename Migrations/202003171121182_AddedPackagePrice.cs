namespace NaijaStartupWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPackagePrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Company_Registration", "PackagePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Company_Registration", "PackagePrice");
        }
    }
}
