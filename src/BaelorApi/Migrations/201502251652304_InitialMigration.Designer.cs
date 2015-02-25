using BaelorApi.Models.Database;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using System;

namespace BaelorApi.Migrations
{
    [ContextType(typeof(BaelorApi.Models.Database.DatabaseContext))]
    public partial class InitialMigration : IMigrationMetadata
    {
        string IMigrationMetadata.MigrationId
        {
            get
            {
                return "201502251652304_InitialMigration";
            }
        }
        
        string IMigrationMetadata.ProductVersion
        {
            get
            {
                return "7.0.0-beta3-12166";
            }
        }
        
        IModel IMigrationMetadata.TargetModel
        {
            get
            {
                var builder = new BasicModelBuilder();
                
                builder.Entity("BaelorApi.Models.Database.Album", b =>
                    {
                        b.Property<DateTime>("CreatedAt");
                        b.Property<string>("Genres");
                        b.Property<Guid>("Id")
                            .GenerateValueOnAdd();
                        b.Property<Guid>("ImageId");
                        b.Property<string>("Label");
                        b.Property<int>("LengthSeconds");
                        b.Property<string>("Name");
                        b.Property<string>("Producers");
                        b.Property<DateTime>("ReleasedAt");
                        b.Property<string>("Slug");
                        b.Property<DateTime>("UpdatedAt");
                        b.Key("Id");
                    });
                
                builder.Entity("BaelorApi.Models.Database.Image", b =>
                    {
                        b.Property<DateTime>("CreatedAt");
                        b.Property<string>("FilePath");
                        b.Property<Guid>("Id")
                            .GenerateValueOnAdd();
                        b.Property<DateTime>("UpdatedAt");
                        b.Key("Id");
                    });
                
                builder.Entity("BaelorApi.Models.Database.RateLimit", b =>
                    {
                        b.Property<DateTime>("CreatedAt");
                        b.Property<Guid>("Id")
                            .GenerateValueOnAdd();
                        b.Property<int>("RequestsMade");
                        b.Property<DateTime>("UpdatedAt");
                        b.Property<Guid>("UserId");
                        b.Key("Id");
                    });
                
                builder.Entity("BaelorApi.Models.Database.Song", b =>
                    {
                        b.Property<Guid>("AlbumId");
                        b.Property<DateTime>("CreatedAt");
                        b.Property<Guid>("Id")
                            .GenerateValueOnAdd();
                        b.Property<int>("Index");
                        b.Property<int>("LengthSeconds");
                        b.Property<string>("Producers");
                        b.Property<string>("Slug");
                        b.Property<string>("Title");
                        b.Property<DateTime>("UpdatedAt");
                        b.Property<string>("Writers");
                        b.Key("Id");
                    });
                
                builder.Entity("BaelorApi.Models.Database.User", b =>
                    {
                        b.Property<string>("ApiKey");
                        b.Property<DateTime>("CreatedAt");
                        b.Property<string>("EmailAddress");
                        b.Property<Guid>("Id")
                            .GenerateValueOnAdd();
                        b.Property<bool>("IsAdmin");
                        b.Property<bool>("IsRevoked");
                        b.Property<string>("PasswordHash");
                        b.Property<int>("PasswordIterations");
                        b.Property<string>("PasswordSalt");
                        b.Property<DateTime>("UpdatedAt");
                        b.Property<string>("Username");
                        b.Key("Id");
                    });
                
                builder.Entity("BaelorApi.Models.Database.Album", b =>
                    {
                        b.ForeignKey("BaelorApi.Models.Database.Image", "ImageId");
                    });
                
                builder.Entity("BaelorApi.Models.Database.RateLimit", b =>
                    {
                        b.ForeignKey("BaelorApi.Models.Database.User", "UserId");
                    });
                
                builder.Entity("BaelorApi.Models.Database.Song", b =>
                    {
                        b.ForeignKey("BaelorApi.Models.Database.Album", "AlbumId");
                    });
                
                return builder.Model;
            }
        }
    }
}