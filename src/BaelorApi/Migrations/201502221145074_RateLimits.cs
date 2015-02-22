using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Model;
using System;

namespace BaelorApi.Migrations
{
    public partial class RateLimits : Migration
    {
        public override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("RateLimit",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreatedAt = c.DateTime(nullable: false),
                        RequestsMade = c.Int(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        UserId = c.Guid(nullable: false)
                    })
                .PrimaryKey("PK_RateLimit", t => t.Id);
            
            migrationBuilder.AddForeignKey("RateLimit", "FK_RateLimit_User_UserId", new[] { "UserId" }, "User", new[] { "Id" }, cascadeDelete: false);
        }
        
        public override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("RateLimit");
        }
    }
}