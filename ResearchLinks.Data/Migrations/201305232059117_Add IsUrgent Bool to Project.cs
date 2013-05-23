namespace ResearchLinks.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsUrgentBooltoProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "IsUrgent", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "IsUrgent");
        }
    }
}
