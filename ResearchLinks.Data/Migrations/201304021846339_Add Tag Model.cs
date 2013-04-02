namespace ResearchLinks.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTagModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        ResearchItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TagId)
                .ForeignKey("dbo.ResearchItems", t => t.ResearchItemId, cascadeDelete: true)
                .Index(t => t.ResearchItemId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tags", new[] { "ResearchItemId" });
            DropForeignKey("dbo.Tags", "ResearchItemId", "dbo.ResearchItems");
            DropTable("dbo.Tags");
        }
    }
}
