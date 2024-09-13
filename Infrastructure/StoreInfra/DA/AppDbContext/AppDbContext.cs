﻿
using DM.DomainModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DA.AppDbContexts
{
    public class AppDbContext:DbContext
    {
        public DbSet<CashManagement> CashManagements { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Till> Till { get; set; }
        public DbSet<CustomerCart> CustomerCart { get; set; }
        public DbSet<CustomerManagement> CustomerManagements { get; set; }
        public DbSet<CustomerCartItems> CustomerCartItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public override int SaveChanges()
        {
            ConvertDateTimesToUtc();
            return base.SaveChanges();
        }

        private void ConvertDateTimesToUtc()
        {
            var entities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity);

            foreach (var entity in entities)
            {
                var properties = entity.GetType().GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

                foreach (var property in properties)
                {
                    if (property.GetValue(entity) is DateTime dateTime)
                    {
                        property.SetValue(entity, dateTime.ToUniversalTime());
                    }

                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ConvertDateTimesToUtc();
            return await base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CustomerCart>(entity => {

                entity.HasOne(x => x.CustomerManagement)
                    .WithMany(x => x.CustomerCarts)
                    .HasForeignKey(x => x.CustomerId);

            });

            builder.Entity<CustomerCartItems>(entity =>
            {
                entity.HasOne(x => x.CustomerCart)
                .WithMany(x => x.CustomerCartItems)
                .HasForeignKey(x => x.CartId);
            });

            builder.Entity<Order>(entity =>
            {
                entity.HasOne(x => x.CustomerCart)
                .WithMany(x => x.Order)
                .HasForeignKey(x => x.CartId);
            });

            builder.Entity<OrderSplitPayments>(entity =>
            {
                entity.HasOne(x => x.Order)
                .WithMany(x => x.OrderSplitPayments)
                .HasForeignKey(x => x.OrderId);

                entity.HasOne(x => x.PaymentMethods)
                .WithMany(x => x.OrderSplitPayments)
                .HasForeignKey(x => x.PaymentMethodId);
            });

            builder.Entity<PaymentTransactions>(entity =>
            {
                entity.HasOne(x => x.OrderSplitPayments)
                .WithMany(x => x.PaymentTransactions)
                .HasForeignKey(x => x.SplitPaymentId);
            });

            builder.Entity<PaymentMethods>(entity =>
            {
                entity.HasData(
                    new PaymentMethods { 
                        Id = Guid.NewGuid().ToString(),
                        Name = "Cash",
                        CreatedBy = "Default",
                        CreatedDate = DateTime.Now.ToUniversalTime(),
                        UpdatedBy = "",
                        UpdatedDate = DateTime.MinValue,
                        IsArchived = false,
                        IsActive = true
                    },
                    new PaymentMethods
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Card",
                        CreatedBy = "Default",
                        CreatedDate = DateTime.Now.ToUniversalTime(),
                        UpdatedBy = "",
                        UpdatedDate = DateTime.MinValue,
                        IsArchived = false,
                        IsActive = true
                    },
                    new PaymentMethods
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Gift Card",
                        CreatedBy = "Default",
                        CreatedDate = DateTime.Now.ToUniversalTime(),
                        UpdatedBy = "",
                        UpdatedDate = DateTime.MinValue,
                        IsArchived = false,
                        IsActive = true
                    }
                );
            });

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        }
    }
}




