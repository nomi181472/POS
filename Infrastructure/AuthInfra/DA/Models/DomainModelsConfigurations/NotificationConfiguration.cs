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
    public class NotificationConfiguration : IEntityTypeConfiguration<NotificationSeen>
    {
        public void Configure(EntityTypeBuilder<NotificationSeen> builder)
        {
            builder.HasOne(x => x.Notification).WithOne(x => x.IsSeen).HasForeignKey<NotificationSeen>(x => x.NotificationId);
        }
    }
}
