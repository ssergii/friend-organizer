namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingPhoneNumber : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhoneNumber",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false),
                        FriendId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Friend", t => t.FriendId, cascadeDelete: true)
                .Index(t => t.FriendId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PhoneNumber", "FriendId", "dbo.Friend");
            DropIndex("dbo.PhoneNumber", new[] { "FriendId" });
            DropTable("dbo.PhoneNumber");
        }
    }
}
