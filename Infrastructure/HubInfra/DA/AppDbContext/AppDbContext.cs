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
        public DbSet<Allowance> Allowances { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Deduction> Deductions { get; set; }
        public DbSet<DeductionRule> DeductionRules { get; set; }
        public DbSet<ShiftDeductionScheduler> ShiftDeductionSchedulers { get; set; }
        public DbSet<AllowanceWorkingProfileManagement> AllowanceWorkingProfileManagements { get; set; }
        public DbSet<Leave> Leaves { get; set; }   
        public DbSet<LeaveWorkingProfileManagement> LeaveWorkingProfileManagements { get; set; }
        public DbSet<WorkingProfile> WorkingProfiles { get; set; }
        public DbSet<FiscalYear> FiscalYears { get; set; }
        public DbSet<ShiftWorkingProfile> shiftWorkingProfiles { get; set; }


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

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        }
    }
}



