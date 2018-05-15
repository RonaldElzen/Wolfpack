namespace Wolfpack.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SkillRating : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.UserRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Double(nullable: false),
                        RatedAt = c.DateTime(nullable: false),
                        RatedBy_Id = c.Int(),
                        RatedQuality_Id = c.Int(),
                        RatedUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.RatedBy_Id)
                .ForeignKey("dbo.Skills", t => t.RatedQuality_Id)
                .ForeignKey("dbo.Users", t => t.RatedUser_Id)
                .Index(t => t.RatedBy_Id)
                .Index(t => t.RatedQuality_Id)
                .Index(t => t.RatedUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRatings", "RatedUser_Id", "dbo.Users");
            DropForeignKey("dbo.UserRatings", "RatedQuality_Id", "dbo.Skills");
            DropForeignKey("dbo.UserRatings", "RatedBy_Id", "dbo.Users");
            DropForeignKey("dbo.Skills", "CreatedBy_Id", "dbo.Users");
            DropIndex("dbo.UserRatings", new[] { "RatedUser_Id" });
            DropIndex("dbo.UserRatings", new[] { "RatedQuality_Id" });
            DropIndex("dbo.UserRatings", new[] { "RatedBy_Id" });
            DropIndex("dbo.Skills", new[] { "CreatedBy_Id" });
            DropTable("dbo.UserRatings");
            DropTable("dbo.Skills");
        }
    }
}
