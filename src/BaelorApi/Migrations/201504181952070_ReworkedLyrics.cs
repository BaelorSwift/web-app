using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.MigrationsModel;
using System;

namespace BaelorApi.Migrations
{
    public partial class ReworkedLyrics : Migration
    {
        public override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("Song", "FK_Song_Lyric_LyricId");
            
            migrationBuilder.DropColumn("Lyric", "Lyrics");
            
            migrationBuilder.DropColumn("Lyric", "Slug");
            
            migrationBuilder.DropColumn("Song", "LyricId");
            
            migrationBuilder.AddColumn("Lyric", "Content", c => c.String());
            
            migrationBuilder.AddColumn("Lyric", "TimeCode", c => c.Int(nullable: false));
            
            migrationBuilder.AddForeignKey(
                "Lyric",
                "FK_Lyric_Song_SongId",
                new[] { "SongId" },
                "Song",
                new[] { "Id" },
                cascadeDelete: false);
        }
        
        public override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("Lyric", "FK_Lyric_Song_SongId");
            
            migrationBuilder.DropColumn("Lyric", "Content");
            
            migrationBuilder.DropColumn("Lyric", "TimeCode");
            
            migrationBuilder.AddColumn("Lyric", "Lyrics", c => c.String());
            
            migrationBuilder.AddColumn("Lyric", "Slug", c => c.String());
            
            migrationBuilder.AddColumn("Song", "LyricId", c => c.Guid());
            
            migrationBuilder.AddForeignKey(
                "Song",
                "FK_Song_Lyric_LyricId",
                new[] { "LyricId" },
                "Lyric",
                new[] { "Id" },
                cascadeDelete: false);
        }
    }
}