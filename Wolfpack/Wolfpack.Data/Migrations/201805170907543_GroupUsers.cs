namespace Wolfpack.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupUsers",
                c => new
                    {
                        Group_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.User_Id })
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Events", "EventCreator_Id", c => c.Int());
            CreateIndex("dbo.Events", "EventCreator_Id");
            AddForeignKey("dbo.Events", "EventCreator_Id", "dbo.Users", "Id");
            DropColumn("dbo.Events", "EventCreator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "EventCreator", c => c.Int(nullable: false));
            DropForeignKey("dbo.Events", "EventCreator_Id", "dbo.Users");
            DropForeignKey("dbo.GroupUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.GroupUsers", "Group_Id", "dbo.Groups");
            DropIndex("dbo.GroupUsers", new[] { "User_Id" });
            DropIndex("dbo.GroupUsers", new[] { "Group_Id" });
            DropIndex("dbo.Events", new[] { "EventCreator_Id" });
            DropColumn("dbo.Events", "EventCreator_Id");
            DropTable("dbo.GroupUsers");
        }
    }
}
