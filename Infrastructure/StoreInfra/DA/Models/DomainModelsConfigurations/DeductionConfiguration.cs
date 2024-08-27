﻿using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Models.DomainModelsConfigurations
{
    public class DeductionConfiguration : IEntityTypeConfiguration<Deduction>
    {
        public void Configure(EntityTypeBuilder<Deduction> builder)
        {
            builder.HasMany(x => x.ShiftDeductionScheduler)
                .WithOne(x => x.Deduction)
                .HasForeignKey(x => x.DeductionId);
        }
    }
}