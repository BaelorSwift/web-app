using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Model;
using System;

namespace BaelorApi.Migrations
{
    public partial class AddedUser : Migration
    {
        public override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("User",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ApiKey = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        Email = c.String(),
                        PasswordHash = c.String(),
                        PasswordIterations = c.Int(nullable: false),
                        PasswordSalt = c.String(),
                        UpdatedAt = c.DateTime(nullable: false),
                        Username = c.String()
                    })
                .PrimaryKey("PK_User", t => t.Id);
            
            migrationBuilder.AddForeignKey("Album", "FK_Album_Image_ImageId", new[] { "ImageId" }, "Image", new[] { "Id" }, cascadeDelete: false);
        }
        
        public override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("Album", "FK_Album_Image_ImageId");
            
            migrationBuilder.DropTable("User");
        }
    }
}