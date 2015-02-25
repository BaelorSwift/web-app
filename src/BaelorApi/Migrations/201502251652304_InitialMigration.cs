using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.MigrationsModel;
using System;

namespace BaelorApi.Migrations
{
    public partial class InitialMigration : Migration
    {
        public override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("Album",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        Genres = c.String(),
                        Label = c.String(),
                        LengthSeconds = c.Int(nullable: false),
                        Name = c.String(),
                        Producers = c.String(),
                        ReleasedAt = c.DateTime(nullable: false),
                        Slug = c.String(),
                        UpdatedAt = c.DateTime(nullable: false),
                        ImageId = c.Guid(nullable: false)
                    })
                .PrimaryKey("PK_Album", t => t.Id);
            
            migrationBuilder.CreateTable("Image",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        FilePath = c.String(),
                        UpdatedAt = c.DateTime(nullable: false)
                    })
                .PrimaryKey("PK_Image", t => t.Id);
            
            migrationBuilder.CreateTable("RateLimit",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        RequestsMade = c.Int(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        UserId = c.Guid(nullable: false)
                    })
                .PrimaryKey("PK_RateLimit", t => t.Id);
            
            migrationBuilder.CreateTable("Song",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        Index = c.Int(nullable: false),
                        LengthSeconds = c.Int(nullable: false),
                        Producers = c.String(),
                        Slug = c.String(),
                        Title = c.String(),
                        UpdatedAt = c.DateTime(nullable: false),
                        Writers = c.String(),
                        AlbumId = c.Guid(nullable: false)
                    })
                .PrimaryKey("PK_Song", t => t.Id);
            
            migrationBuilder.CreateTable("User",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApiKey = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        EmailAddress = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                        IsRevoked = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        PasswordIterations = c.Int(nullable: false),
                        PasswordSalt = c.String(),
                        UpdatedAt = c.DateTime(nullable: false),
                        Username = c.String()
                    })
                .PrimaryKey("PK_User", t => t.Id);
            
            migrationBuilder.AddForeignKey(
                "Album",
                "FK_Album_Image_ImageId",
                new[] { "ImageId" },
                "Image",
                new[] { "Id" },
                cascadeDelete: false);
            
            migrationBuilder.AddForeignKey(
                "RateLimit",
                "FK_RateLimit_User_UserId",
                new[] { "UserId" },
                "User",
                new[] { "Id" },
                cascadeDelete: false);
            
            migrationBuilder.AddForeignKey(
                "Song",
                "FK_Song_Album_AlbumId",
                new[] { "AlbumId" },
                "Album",
                new[] { "Id" },
                cascadeDelete: false);
        }
        
        public override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("Song", "FK_Song_Album_AlbumId");
            
            migrationBuilder.DropForeignKey("Album", "FK_Album_Image_ImageId");
            
            migrationBuilder.DropForeignKey("RateLimit", "FK_RateLimit_User_UserId");
            
            migrationBuilder.DropTable("Album");
            
            migrationBuilder.DropTable("Image");
            
            migrationBuilder.DropTable("RateLimit");
            
            migrationBuilder.DropTable("Song");
            
            migrationBuilder.DropTable("User");
        }
    }
}