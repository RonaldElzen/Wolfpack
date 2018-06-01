namespace Wolfpack.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RatingComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserRatings", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserRatings", "Comment");
        }
    }
}
