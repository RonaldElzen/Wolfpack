namespace Wolfpack.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Recovery : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Recoveries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recoveries", "User_Id", "dbo.Users");
            DropIndex("dbo.Recoveries", new[] { "User_Id" });
            DropTable("dbo.Recoveries");
        }
    }
}
