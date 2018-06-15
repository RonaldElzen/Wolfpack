namespace Wolfpack.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewRegister : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewRegisters", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NewRegisters", "Email");
        }
    }
}
