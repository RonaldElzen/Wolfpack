namespace Wolfpack.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Group_Id", c => c.Int());
            CreateIndex("dbo.Events", "Group_Id");
            AddForeignKey("dbo.Events", "Group_Id", "dbo.Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "Group_Id", "dbo.Groups");
            DropIndex("dbo.Events", new[] { "Group_Id" });
            DropColumn("dbo.Events", "Group_Id");
        }
    }
}
