namespace Wolfpack.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SkillsInEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Skills", "Event_Id", c => c.Int());
            CreateIndex("dbo.Skills", "Event_Id");
            AddForeignKey("dbo.Skills", "Event_Id", "dbo.Events", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Skills", "Event_Id", "dbo.Events");
            DropIndex("dbo.Skills", new[] { "Event_Id" });
            DropColumn("dbo.Skills", "Event_Id");
        }
    }
}
