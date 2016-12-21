namespace NbuReservationSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddedUniqueIndexes : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Reservations", new[] { "Date", "BeginsOn", "EndsOn" }, unique: true, name: "IX_ReservationUniqueness");
        }

        public override void Down()
        {
            DropIndex("dbo.Reservations", "IX_ReservationUniqueness");
        }
    }
}
