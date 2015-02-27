using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.MigrationsModel;
using System;

namespace BaelorApi.Migrations
{
    public partial class LyricSlug : Migration
    {
        public override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("Song", "FK_Song_Lyric_LyricId");
            
            migrationBuilder.AlterColumn("Lyric", "SongId", c => c.Guid());
            
            migrationBuilder.AlterColumn("Song", "LyricId", c => c.Guid());
            
            migrationBuilder.AddColumn("Lyric", "Slug", c => c.String());
            
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
            
            migrationBuilder.DropColumn("Lyric", "Slug");
            
            migrationBuilder.AlterColumn("Lyric", "SongId", c => c.Guid(nullable: false));
            
            migrationBuilder.AlterColumn("Song", "LyricId", c => c.Guid(nullable: false));
            
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