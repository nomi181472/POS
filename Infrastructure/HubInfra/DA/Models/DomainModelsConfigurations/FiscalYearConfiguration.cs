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
    public class FiscalYearConfiguration : IEntityTypeConfiguration<FiscalYear>
    {
        public void Configure(EntityTypeBuilder<FiscalYear> builder)
        {
            builder.HasMany(x => x.WorkingProfiles)
                .WithOne(x => x.FiscalYear)
                .HasForeignKey(x => x.FiscalYearId);
        }
    }
}
