using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationExtenison.EntityFrameworkCore
{
    public class TableConfiguration
    {


        public TableConfiguration()
        {

        }
        public TableConfiguration(string name, string schema)
        : this(name, schema, null)
        {
        }
        public TableConfiguration(string name)
        : this(name, null, null)
        {
        }

        public TableConfiguration(Action<EntityTypeBuilder> buildAction)
        : this(null, buildAction)
        {
        }

        public TableConfiguration(string name, Action<EntityTypeBuilder> buildAction)
        : this(name, null, buildAction)
        {
        }
        public TableConfiguration(string name, string schema, Action<EntityTypeBuilder> buildAction)
        {
            BuildAction = buildAction;
            Name = name;
            Schema = schema;
        }

        public string Name { get; set; } 
        public string Schema { get; set; }
        public Action<EntityTypeBuilder> BuildAction { get; set; }


    }
}