namespace Wolfpack.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LockedAccounts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LockedAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Users", "LoginAttempts", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LockedAccounts", "User_Id", "dbo.Users");
            DropIndex("dbo.LockedAccounts", new[] { "User_Id" });
            DropColumn("dbo.Users", "LoginAttempts");
            DropTable("dbo.LockedAccounts");
        }
    }
}
