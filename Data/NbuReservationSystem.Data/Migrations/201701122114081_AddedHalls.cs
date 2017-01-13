namespace NbuReservationSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHalls : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Halls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Color = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);
            
            AddColumn("dbo.Reservations", "Hall_Id", c => c.Int());
            CreateIndex("dbo.Reservations", "Hall_Id");
            AddForeignKey("dbo.Reservations", "Hall_Id", "dbo.Halls", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "Hall_Id", "dbo.Halls");
            DropIndex("dbo.Reservations", new[] { "Hall_Id" });
            DropIndex("dbo.Halls", new[] { "IsDeleted" });
            DropColumn("dbo.Reservations", "Hall_Id");
            DropTable("dbo.Halls");
        }
    }
}
