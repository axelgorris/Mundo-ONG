namespace NGODirectory.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAnnouncementAuthorField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "Author", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Announcements", "Author");
        }
    }
}
