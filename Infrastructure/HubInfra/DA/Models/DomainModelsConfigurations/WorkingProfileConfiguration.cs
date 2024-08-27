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
    public class WorkingProfileConfiguration : IEntityTypeConfiguration<WorkingProfile>
    {
        public void Configure(EntityTypeBuilder<WorkingProfile> builder)
        {
            builder.HasMany(x => x.LeaveWorkingProfileManagements)
                .WithOne(x=>x.WorkingProfile)
                .HasForeignKey(x=>x.WorkingProfileId);


            builder.HasMany(x => x.AllowanceWorkingProfileManagements)
               .WithOne(x => x.WorkingProfile)
               .HasForeignKey(x => x.WorkingProfileId);

            builder.HasMany(x => x.ShiftDeductionScheduler)
                .WithOne(x => x.WorkingProfile)
                .HasForeignKey(x => x.WorkingProfileId);

        }
    }
}
