using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Migrations.Builders;

namespace BaelorApi.Migrations
{
    public partial class AddedAlbumsBro : Migration
    {
        public override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("Album",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreatedAt = c.DateTime(nullable: false),
                        Label = c.String(),
                        LengthSeconds = c.Int(nullable: false),
                        Name = c.String(),
                        ReleasedAt = c.DateTime(nullable: false),
                        Slug = c.String(),
                        UpdatedAt = c.DateTime(nullable: false)
                    })
                .PrimaryKey("PK_Album", t => t.Id);
        }
        
        public override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Album");
        }
    }
}