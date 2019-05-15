namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingProgrammingLanguage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgrammingLanguage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Friend", "FavoritLanguageId", c => c.Int());
            CreateIndex("dbo.Friend", "FavoritLanguageId");
            AddForeignKey("dbo.Friend", "FavoritLanguageId", "dbo.ProgrammingLanguage", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friend", "FavoritLanguageId", "dbo.ProgrammingLanguage");
            DropIndex("dbo.Friend", new[] { "FavoritLanguageId" });
            DropColumn("dbo.Friend", "FavoritLanguageId");
            DropTable("dbo.ProgrammingLanguage");
        }
    }
}
