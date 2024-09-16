using DA.Common.CommonRoles;
using DM.DomainModels;
using Helpers.StringsExtension;
using Microsoft.AspNetCore.Identity;
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
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Actions> Actions { get; set; }
        public DbSet<RoleAction> RoleActions { get; set; }
        public DbSet<UserRole> userRoles { get; set; }
        public DbSet<Credential> Credential { get; set; }



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
            SeedSuperAdmin(builder);
            
        }
        private void SeedSuperAdmin(ModelBuilder builder)
        {
            // Create the super admin role
            string userId = "72990663-2edc-4c10-b331-cd1c65e477e0";
            var roleId = "78f4b56a-3fa3-4067-b641-7adb0a7a2ca7";
            var credsId = "10bcc73c-e40a-4852-a81d-c18d654e8806";
            var userRoleId = "f3df99c7-07fb-4b7f-89e5-89d86b84bd4e";
            var now = DateTime.UtcNow;
            var adminRole = new Role(roleId,userId,now, KDefinedRoles.SuperAdmin);
            

            
            //TODO Will be taking from env variable
            string pass = "Pos!23";
            var helper = PasswordHelper.HashPassword(pass);
            string pH = helper.hash;
            string pS = helper.salt;

            var userRoles=new List<UserRole>() { new UserRole(userId, roleId, userRoleId, userId, now) };
            var credential = new Credential(credsId, userId, now,pPasswordHash:pH,pPasswordSalt:pS,pUserId: userId);
            var adminUser = new User(userId,userId,now,"POS","POS@gmail.com", KDefinedRoles.SuperAdmin
               
                );

            //builders
            builder.Entity<User>().HasData(adminUser);
            builder.Entity<Credential>().HasData(credential);
            builder.Entity<UserRole>().HasData(userRoles);
            builder.Entity<Role>().HasData(adminRole);
           



        }
    }
}




