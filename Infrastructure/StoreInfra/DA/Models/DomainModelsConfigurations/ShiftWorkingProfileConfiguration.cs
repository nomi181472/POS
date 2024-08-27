using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DA.Models.DomainModelsConfigurations
{
    public class ShiftWorkingProfileConfiguration : IEntityTypeConfiguration<ShiftWorkingProfile>
    {
        public void Configure(EntityTypeBuilder<ShiftWorkingProfile> builder)
        {
            builder.HasOne(swp => swp.Shift)
                .WithMany(s => s.ShiftWorkingProfile)
                .HasForeignKey(swp => swp.ShiftId);

            builder.HasOne(swp => swp.WorkingProfile)
                .WithMany(wp => wp.ShiftWorkingProfile)
                .HasForeignKey(swp => swp.WorkingProfileId);
        }
    }
}
