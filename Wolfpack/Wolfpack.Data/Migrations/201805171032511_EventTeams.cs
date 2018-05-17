namespace Wolfpack.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventTeams : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventTeams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Event_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.Event_Id);
            
            CreateTable(
                "dbo.EventTeamUsers",
                c => new
                    {
                        EventTeam_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventTeam_Id, t.User_Id })
                .ForeignKey("dbo.EventTeams", t => t.EventTeam_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.EventTeam_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventTeamUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.EventTeamUsers", "EventTeam_Id", "dbo.EventTeams");
            DropForeignKey("dbo.EventTeams", "Event_Id", "dbo.Events");
            DropIndex("dbo.EventTeamUsers", new[] { "User_Id" });
            DropIndex("dbo.EventTeamUsers", new[] { "EventTeam_Id" });
            DropIndex("dbo.EventTeams", new[] { "Event_Id" });
            DropTable("dbo.EventTeamUsers");
            DropTable("dbo.EventTeams");
        }
    }
}
