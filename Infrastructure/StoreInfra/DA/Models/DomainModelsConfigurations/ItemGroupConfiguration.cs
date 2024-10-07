using DM.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Models.DomainModelsConfigurations
{
    public class ItemGroupConfiguration
    {
        public void Configure(EntityTypeBuilder<ItemGroup> builder)
        {
            builder.HasMany(x => x.Items).WithOne(Items => Items.ItemGroup).HasForeignKey(x => x.ItemGroupId);
        }
    }
}
