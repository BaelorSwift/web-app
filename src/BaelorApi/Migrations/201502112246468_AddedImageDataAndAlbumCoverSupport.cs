using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Migrations.Builders;
using System;

namespace BaelorApi.Migrations
{
    public partial class AddedImageDataAndAlbumCoverSupport : Migration
    {
        public override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("Image",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreatedAt = c.DateTime(nullable: false),
                        FilePath = c.String(),
                        UpdatedAt = c.DateTime(nullable: false)
                    })
                .PrimaryKey("PK_Image", t => t.Id);
            
            migrationBuilder.AddColumn("Album", "ImageId", c => c.Guid(nullable: false, defaultValue: Guid.NewGuid()));
        }
        
        public override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Album", "ImageId");
            
            migrationBuilder.DropTable("Image");
        }
    }
}