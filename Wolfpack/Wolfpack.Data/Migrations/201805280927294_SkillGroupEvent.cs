namespace Wolfpack.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SkillGroupEvent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Skills", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Skills", "Event_Id", "dbo.Events");
            DropIndex("dbo.Skills", new[] { "Group_Id" });
            DropIndex("dbo.Skills", new[] { "Event_Id" });
            DropColumn("dbo.Skills", "Group_Id");
            DropColumn("dbo.Skills", "Event_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Skills", "Event_Id", c => c.Int());
            AddColumn("dbo.Skills", "Group_Id", c => c.Int());
            CreateIndex("dbo.Skills", "Event_Id");
            CreateIndex("dbo.Skills", "Group_Id");
            AddForeignKey("dbo.Skills", "Event_Id", "dbo.Events", "Id");
            AddForeignKey("dbo.Skills", "Group_Id", "dbo.Groups", "Id");
        }
    }
}
