using DM.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Models.DomainModelsConfigurations
{
    public class InventoryGroupsConfiguration : IEntityTypeConfiguration<InventoryGroups>
    {
        public void Configure(EntityTypeBuilder<InventoryGroups> builder)
        {
            
        }
    }
}
