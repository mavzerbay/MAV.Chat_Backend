using MAV.Chat.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Reflection;
using System.Security.Claims;

namespace MAV.Chat.Infrastructure.Context
{
    public class MavDbContext : IdentityDbContext<MavUser, MavRole, int,
      IdentityUserClaim<int>, MavUserRole, IdentityUserLogin<int>,
      IdentityRoleClaim<int>, IdentityUserToken<int>>
    {

        private IHttpContextAccessor _httpContextAccessor => new HttpContextAccessor();
        public MavDbContext(DbContextOptions<MavDbContext> options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.ApplyUtcDateTimeConverter();
        }

        public override int SaveChanges()
        {
            this.ChangeTracker.DetectChanges();

            foreach (var entity in this.ChangeTracker.Entries())
            {
                string userId = string.Empty;
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                    userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                switch (entity.State)
                {
                    case EntityState.Added:
                        if (entity.GetType().GetProperty("CreatedById") != null && !string.IsNullOrEmpty(userId))
                            entity.GetType().GetProperty("CreatedById").SetValue(entity, int.Parse(userId));
                        if (entity.GetType().GetProperty("CreatedDate") != null)
                            entity.GetType().GetProperty("CreatedDate").SetValue(entity, DateTime.Now);
                        break;
                    case EntityState.Modified:
                        if (entity.GetType().GetProperty("UpdatedById") != null && !string.IsNullOrEmpty(userId))
                            entity.GetType().GetProperty("UpdatedById").SetValue(entity, int.Parse(userId));
                        if (entity.GetType().GetProperty("UpdatedDate") != null)
                            entity.GetType().GetProperty("UpdatedDate").SetValue(entity, DateTime.Now);
                        break;
                    case EntityState.Deleted:
                        if (entity.GetType().GetProperty("DeletedById") != null && !string.IsNullOrEmpty(userId))
                            entity.GetType().GetProperty("DeletedById").SetValue(entity, int.Parse(userId));
                        if (entity.GetType().GetProperty("DeletedDate") != null)
                            entity.GetType().GetProperty("DeletedDate").SetValue(entity, DateTime.Now);
                        break;
                }

            }

            return base.SaveChanges();
        }
    }

    public static class UtcDateAnnotation
    {
        private const String IsUtcAnnotation = "IsUtc";
        private static readonly ValueConverter<DateTime, DateTime> UtcConverter =
          new ValueConverter<DateTime, DateTime>(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        private static readonly ValueConverter<DateTime?, DateTime?> UtcNullableConverter =
          new ValueConverter<DateTime?, DateTime?>(v => v, v => v == null ? v : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));

        public static PropertyBuilder<TProperty> IsUtc<TProperty>(this PropertyBuilder<TProperty> builder, Boolean isUtc = true) =>
          builder.HasAnnotation(IsUtcAnnotation, isUtc);

        public static Boolean IsUtc(this IMutableProperty property) =>
          ((Boolean?)property.FindAnnotation(IsUtcAnnotation)?.Value) ?? true;

        /// <summary>
        /// Make sure this is called after configuring all your entities.
        /// </summary>
        public static void ApplyUtcDateTimeConverter(this ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (!property.IsUtc())
                    {
                        continue;
                    }

                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(UtcConverter);
                    }

                    if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(UtcNullableConverter);
                    }
                }
            }
        }
    }
}
