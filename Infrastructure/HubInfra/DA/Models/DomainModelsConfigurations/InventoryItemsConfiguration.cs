﻿using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Models.DomainModelsConfigurations
{
    public class InventoryItemsConfiguration : IEntityTypeConfiguration<InventoryItems>
    {
        public void Configure(EntityTypeBuilder<InventoryItems> builder)
        {
            builder.HasOne(i => i.InventoryGroups)              
                .WithMany(g => g.InventoryItems)             
                .HasForeignKey(i => i.GroupId);
        }
    }
}
