using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Model;
using System;

namespace BaelorApi.Migrations
{
    public partial class SongsBro : Migration
    {
        public override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("Song",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
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
            
            migrationBuilder.AddForeignKey("Song", "FK_Song_Album_AlbumId", new[] { "AlbumId" }, "Album", new[] { "Id" }, cascadeDelete: false);
        }
        
        public override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Song");
        }
    }
}