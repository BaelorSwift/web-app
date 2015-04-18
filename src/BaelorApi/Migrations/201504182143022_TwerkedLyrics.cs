using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.MigrationsModel;
using System;

namespace BaelorApi.Migrations
{
    public partial class TwerkedLyrics : Migration
    {
        public override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn("Lyric", "TimeCode", c => c.Single(nullable: false));
        }
        
        public override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn("Lyric", "TimeCode", c => c.Int(nullable: false));
        }
    }
}