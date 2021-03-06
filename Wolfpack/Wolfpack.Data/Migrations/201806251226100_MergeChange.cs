namespace Wolfpack.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MergeChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        Date = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Groups", "Archived", c => c.Boolean(nullable: false));

            CreateTable(
                "dbo.NewRegisters",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Key = c.String(),
                    GroupId = c.Int(nullable: false),
                    Email = c.String(),
                })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.NewRegisters");
            DropForeignKey("dbo.Notifications", "User_Id", "dbo.Users");
            DropIndex("dbo.Notifications", new[] { "User_Id" });
            DropColumn("dbo.Groups", "Archived");
            DropTable("dbo.Notifications");
        }
    }
}
