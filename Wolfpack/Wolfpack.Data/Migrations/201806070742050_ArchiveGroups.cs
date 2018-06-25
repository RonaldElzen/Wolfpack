namespace Wolfpack.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArchiveGroups : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Archived", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "Archived");
        }
    }
}
