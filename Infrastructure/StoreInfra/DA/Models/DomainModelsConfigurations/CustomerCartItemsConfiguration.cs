﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Models.DomainModelsConfigurations
{
    public class CustomerCartItemsConfiguration
    {
        public void Configure(EntityTypeBuilder<CustomerCartItems> builder)
        {

        }
    }
}