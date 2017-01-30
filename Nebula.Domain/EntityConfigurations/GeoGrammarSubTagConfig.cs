using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nebula.Domain.Entities;

namespace Nebula.Domain.EntityConfigurations
{
    public class GeoGrammarSubTagConfig : EntityTypeConfiguration<GeoGrammarSubTag>
    {
        public GeoGrammarSubTagConfig()
        {
            HasRequired(a => a.GeoGrammarTag)
                .WithMany(a => a.GeoGrammarSubTags)
                .HasForeignKey(a => a.GeoGrammarTagId)
                .WillCascadeOnDelete(true);
        }
    }
}
