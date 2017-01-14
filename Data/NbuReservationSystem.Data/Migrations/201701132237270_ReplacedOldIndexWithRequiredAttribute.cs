namespace NbuReservationSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReplacedOldIndexWithRequiredAttribute : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Reservations", "IX_ReservationUniqueness");
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Reservations", new[] { "Date", "StartHour", "EndHour" }, unique: true, name: "IX_ReservationUniqueness");
        }
    }
}
