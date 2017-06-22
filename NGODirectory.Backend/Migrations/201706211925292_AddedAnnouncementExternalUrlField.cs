namespace NGODirectory.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAnnouncementExternalUrlField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "ExternalUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Announcements", "ExternalUrl");
        }
    }
}
