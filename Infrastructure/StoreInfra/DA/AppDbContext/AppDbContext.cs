
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
        public DbSet<CashSession> CashSessions { get; set; }
        public DbSet<CashDetails> CashDetails  { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<ItemGroup> ItemGroups { get; set; }

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
                        Id = "f69246eb-c27e-4964-bb11-373baab02634",
                        Name = "Cash",
                        CreatedBy = "Default",
                        CreatedDate = Convert.ToDateTime("2024-09-16").ToUniversalTime(),
                        UpdatedBy = "",
                        UpdatedDate = DateTime.MinValue,
                        IsArchived = false,
                        IsActive = true
                    },
                    new PaymentMethods
                    {
                        Id = "c2d15ca4-04f9-4531-9645-ccfd75429252",
                        Name = "Card",
                        CreatedBy = "Default",
                        CreatedDate = Convert.ToDateTime("2024-09-16").ToUniversalTime(),
                        UpdatedBy = "",
                        UpdatedDate = DateTime.MinValue,
                        IsArchived = false,
                        IsActive = true
                    },
                    new PaymentMethods
                    {
                        Id = "bb8b2f52-f3da-4704-80e3-db641c337bb5",
                        Name = "Gift Card",
                        CreatedBy = "Default",
                        CreatedDate = Convert.ToDateTime("2024-09-16").ToUniversalTime(),
                        UpdatedBy = "",
                        UpdatedDate = DateTime.MinValue,
                        IsArchived = false,
                        IsActive = true
                    },
                    new PaymentMethods
                    {
                        Id = "a5fc46ab-35c4-4ac6-9773-e4bdff743f94",
                        Name = "Cheque",
                        CreatedBy = "Default",
                        CreatedDate = Convert.ToDateTime("2024-09-16").ToUniversalTime(),
                        UpdatedBy = "",
                        UpdatedDate = DateTime.MinValue,
                        IsArchived = false,
                        IsActive = true
                    },
                    new PaymentMethods
                    {
                        Id = "a0977129-9878-499e-a912-4156a7ead1c9",
                        Name = "Online Transfer",
                        CreatedBy = "Default",
                        CreatedDate = Convert.ToDateTime("2024-09-16").ToUniversalTime(),
                        UpdatedBy = "",
                        UpdatedDate = DateTime.MinValue,
                        IsArchived = false,
                        IsActive = true
                    },
                    new PaymentMethods
                    {
                        Id = "647a1871-3dbe-434b-bd2c-877cecdeb964",
                        Name = "Loyalty Points",
                        CreatedBy = "Default",
                        CreatedDate = Convert.ToDateTime("2024-09-16").ToUniversalTime(),
                        UpdatedBy = "",
                        UpdatedDate = DateTime.MinValue,
                        IsArchived = false,
                        IsActive = true
                    },
                    new PaymentMethods
                    {
                        Id = "9fabcd90-1a72-4825-b813-f821c088bac6",
                        Name = "Discount Voucher",
                        CreatedBy = "Default",
                        CreatedDate = Convert.ToDateTime("2024-09-16").ToUniversalTime(),
                        UpdatedBy = "",
                        UpdatedDate = DateTime.MinValue,
                        IsArchived = false,
                        IsActive = true
                    }
                );
            });

            builder.Entity<Items>(entity =>
            {
                
                entity.HasMany(x => x.CustomerCartItems).WithOne(x=>x.Items).HasForeignKey(x => x.ItemId);
            });

            builder.Entity<Tax>(entity =>
            {
                entity.HasOne(x => x.Items).WithOne(y => y.Taxes).HasForeignKey<Tax>(t => t.ItemId);
            });

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        }
    }
}




