using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Model;
using System;

namespace BaelorApi.Migrations
{
    public partial class UpdatedAlbums : Migration
    {
        public override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn("Album", "LengthSeconds", c => c.Int(nullable: false));
            
            migrationBuilder.AddColumn("Album", "Genres", c => c.String());
            
            migrationBuilder.AddColumn("Album", "Producers", c => c.String());
        }
        
        public override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Album", "Genres");
            
            migrationBuilder.DropColumn("Album", "Producers");
            
            migrationBuilder.AlterColumn("Album", "LengthSeconds", c => c.Int(nullable: false));
        }
    }
}