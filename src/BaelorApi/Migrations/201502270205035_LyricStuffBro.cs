using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.MigrationsModel;
using System;

namespace BaelorApi.Migrations
{
    public partial class LyricStuffBro : Migration
    {
        public override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn("Song", "LyricId", c => c.Guid(nullable: true));
            
            migrationBuilder.CreateTable("Lyric",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        Lyrics = c.String(),
                        SongId = c.Guid(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false)
                    })
                .PrimaryKey("PK_Lyric", t => t.Id);
            
            migrationBuilder.AddForeignKey(
                "Song",
                "FK_Song_Lyric_LyricId",
                new[] { "LyricId" },
                "Lyric",
                new[] { "Id" },
                cascadeDelete: false);
        }
        
        public override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("Song", "FK_Song_Lyric_LyricId");
            
            migrationBuilder.DropColumn("Song", "LyricId");
            
            migrationBuilder.DropTable("Lyric");
        }
    }
}