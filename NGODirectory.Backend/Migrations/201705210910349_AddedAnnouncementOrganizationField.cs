namespace NGODirectory.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAnnouncementOrganizationField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "Organization", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Announcements", "Organization");
        }
    }
}
