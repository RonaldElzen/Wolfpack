namespace Wolfpack.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SkillRatingsRework : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserRatings", "RatedBy_Id", "dbo.Users");
            DropForeignKey("dbo.UserRatings", "RatedQuality_Id", "dbo.Skills");
            DropForeignKey("dbo.UserRatings", "RatedUser_Id", "dbo.Users");
            DropIndex("dbo.UserRatings", new[] { "RatedBy_Id" });
            DropIndex("dbo.UserRatings", new[] { "RatedQuality_Id" });
            DropIndex("dbo.UserRatings", new[] { "RatedUser_Id" });
            CreateTable(
                "dbo.UserSkills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Skill_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Skills", t => t.Skill_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Skill_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mark = c.Double(nullable: false),
                        RatedAt = c.DateTime(nullable: false),
                        RatedBy_Id = c.Int(),
                        UserSkill_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.RatedBy_Id)
                .ForeignKey("dbo.UserSkills", t => t.UserSkill_Id)
                .Index(t => t.RatedBy_Id)
                .Index(t => t.UserSkill_Id);
            
            DropColumn("dbo.Skills", "CreatedAt");
            DropTable("dbo.UserRatings");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Skills", "CreatedAt", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.UserSkills", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserSkills", "Skill_Id", "dbo.Skills");
            DropForeignKey("dbo.Ratings", "UserSkill_Id", "dbo.UserSkills");
            DropForeignKey("dbo.Ratings", "RatedBy_Id", "dbo.Users");
            DropIndex("dbo.Ratings", new[] { "UserSkill_Id" });
            DropIndex("dbo.Ratings", new[] { "RatedBy_Id" });
            DropIndex("dbo.UserSkills", new[] { "User_Id" });
            DropIndex("dbo.UserSkills", new[] { "Skill_Id" });
            DropTable("dbo.Ratings");
            DropTable("dbo.UserSkills");
            CreateIndex("dbo.UserRatings", "RatedUser_Id");
            CreateIndex("dbo.UserRatings", "RatedQuality_Id");
            CreateIndex("dbo.UserRatings", "RatedBy_Id");
            AddForeignKey("dbo.UserRatings", "RatedUser_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.UserRatings", "RatedQuality_Id", "dbo.Skills", "Id");
            AddForeignKey("dbo.UserRatings", "RatedBy_Id", "dbo.Users", "Id");
        }
    }
}
