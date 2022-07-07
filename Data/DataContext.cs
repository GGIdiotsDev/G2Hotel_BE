using g2hotel_server.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace g2hotel_server.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<RoomType> RoomTypes { get; set; } = null!;
        public DbSet<Photo> Photos { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<DetailRoomPayment> DetailRoomPayments { get; set; } = null!;
        public DbSet<DetailServicePayment> DetailServicePayments { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<PaymentType> PaymentTypes { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<Room>()
                .HasIndex(r => r.Code)
                .IsUnique();

            builder.Entity<DetailServicePayment>().HasKey(dsp => new { dsp.PaymentId, dsp.ServiceId });
            builder.Entity<DetailRoomPayment>().HasKey(drp => new { drp.PaymentId, drp.RoomId });

            //seed payment types data
            builder.Entity<PaymentType>().HasData(
                new PaymentType { Id = 1, Name = "Cash" },
                new PaymentType { Id = 2, Name = "Credit Card" },
                new PaymentType { Id = 3, Name = "Debit Card" },
                new PaymentType { Id = 4, Name = "Momo" }
            );

            //seed rooms data
            builder.Entity<Room>().HasData(
                new Room { Id = 1, Name = "Room 1", Code = "ROOM-01", DefaultPrice = 100000, Description = "Room 1 description" },
                new Room { Id = 2, Name = "Room 2", Code = "ROOM-02", DefaultPrice = 200000, Description = "Room 2 description" },
                new Room { Id = 3, Name = "Room 3", Code = "ROOM-03", DefaultPrice = 300000, Description = "Room 3 description" },
                new Room { Id = 4, Name = "Room 4", Code = "ROOM-04", DefaultPrice = 400000, Description = "Room 4 description" }
            );

            builder.ApplyUtcDateTimeConverter();
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