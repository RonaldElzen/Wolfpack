namespace Wolfpack.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegisterInvite : DbMigration
    {
        public override void Up()
        {
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
        }
    }
}
