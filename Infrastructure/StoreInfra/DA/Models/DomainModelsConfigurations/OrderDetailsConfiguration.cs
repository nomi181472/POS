﻿using DM.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Models.DomainModelsConfigurations
{
    public class OrderDetailsConfiguration
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {

        }
    }
}