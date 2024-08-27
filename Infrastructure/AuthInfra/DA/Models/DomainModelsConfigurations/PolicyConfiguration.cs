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
    public class AllowanceConfiguration : IEntityTypeConfiguration<Policy>
    {

        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.HasMany(x => x.RolePolicies)
                .WithOne(x => x.Policy)
                .HasForeignKey(x => x.PolicyId);
        }
    }
}
