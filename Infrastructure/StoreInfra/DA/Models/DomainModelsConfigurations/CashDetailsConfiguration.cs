using DM.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Models.DomainModelsConfigurations
{
    public class CashDetailsConfiguration
    {
        public void Configure(EntityTypeBuilder<CashDetails> builder)
        {
            builder.HasOne(x => x.CashSession).WithMany(x => x.cashDetails).HasForeignKey(x => x.CashSessionId);
        }
    }
}
