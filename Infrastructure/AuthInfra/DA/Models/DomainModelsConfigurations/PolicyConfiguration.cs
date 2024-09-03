using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Models.DomainModelsConfigurations
{
    public class PolicyConfiguration : IEntityTypeConfiguration<Actions>
    {

        public void Configure(EntityTypeBuilder<Actions> builder)
        {
            builder.HasMany(x => x.RoleActions)
                .WithOne(x => x.Policy)
                .HasForeignKey(x => x.ActionId);
        }
    }
}
