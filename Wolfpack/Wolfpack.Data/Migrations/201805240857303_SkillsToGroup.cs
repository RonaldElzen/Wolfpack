namespace Wolfpack.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SkillsToGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Skills", "Group_Id", c => c.Int());
            CreateIndex("dbo.Skills", "Group_Id");
            AddForeignKey("dbo.Skills", "Group_Id", "dbo.Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Skills", "Group_Id", "dbo.Groups");
            DropIndex("dbo.Skills", new[] { "Group_Id" });
            DropColumn("dbo.Skills", "Group_Id");
        }
    }
}
