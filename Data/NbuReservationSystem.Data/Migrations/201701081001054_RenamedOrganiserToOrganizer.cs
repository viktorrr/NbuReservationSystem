namespace NbuReservationSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class RenamedOrganiserToOrganizer : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Organisers", newName: "Organizers");
            RenameColumn(table: "dbo.Reservations", name: "Organiser_Id", newName: "OrganizerId");
            RenameIndex(table: "dbo.Reservations", name: "IX_Organiser_Id", newName: "IX_OrganizerId");
            DropColumn("dbo.Reservations", "OrganaiserId");
        }

        public override void Down()
        {
            AddColumn("dbo.Reservations", "OrganaiserId", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.Reservations", name: "IX_OrganizerId", newName: "IX_Organiser_Id");
            RenameColumn(table: "dbo.Reservations", name: "OrganizerId", newName: "Organiser_Id");
            RenameTable(name: "dbo.Organizers", newName: "Organisers");
        }
    }
}
