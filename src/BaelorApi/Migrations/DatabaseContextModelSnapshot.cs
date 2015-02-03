using BaelorApi.Models.Database;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations.Infrastructure;
using System;

namespace BaelorApi.Migrations
{
    [ContextType(typeof(DatabaseContext))]
    public class DatabaseContextModelSnapshot : ModelSnapshot
    {
        public override IModel Model
        {
            get
            {
                var builder = new BasicModelBuilder();
                
                builder.Entity("BaelorApi.Models.Database.Album", b =>
                    {
                        b.Property<DateTime>("CreatedAt");
                        b.Property<Guid>("Id")
                            .GenerateValuesOnAdd();
                        b.Property<string>("Label");
                        b.Property<uint>("LengthSeconds");
                        b.Property<string>("Name");
                        b.Property<DateTime>("ReleasedAt");
                        b.Property<string>("Slug");
                        b.Property<DateTime>("UpdatedAt");
                        b.Key("Id");
                    });
                
                return builder.Model;
            }
        }
    }
}