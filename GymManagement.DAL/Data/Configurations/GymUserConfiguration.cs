using GymManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Configurations
{
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            builder.Property(x => x.Phone)
                .HasColumnType("varchar")
                .HasMaxLength(11);


            string tableName = typeof(T).Name;

            // ToTable() with a lambda gives access to table-level settings.
            // HasCheckConstraint() adds a SQL condition enforced by the DATABASE ENGINE itself,
            // not by C#. Even if someone writes directly to the DB, these rules still block invalid data.
            builder.ToTable(Tb =>
            {
                // Email must contain '@' and a '.' to be valid.
                Tb.HasCheckConstraint($"CK_{tableName}_Email", "Email Like '_%@_%._%'");


                // Phone must start with a valid Egyptian prefix (010, 011, 012, 015)
                // AND must contain digits only (no letters or special characters).
                Tb.HasCheckConstraint($"CK_{tableName}_Phone", "(Phone Like '010%' OR Phone Like '011%' OR Phone Like '012%' OR Phone Like '015%') AND Phone Not Like '%[^0-9]%'");
            });




            // HasIndex() speeds up search queries on these columns.
            // IsUnique() tells the DB to reject duplicate values —
            // inserting the same Email or Phone twice will throw an exception.
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Phone).IsUnique();


            // OwnsOne() tells EF Core that Address is NOT a separate table.
            // Its properties (Street, City, BuildingNumber) are flattened as regular columns
            // inside the same Members/Trainers table — no Foreign Key, no second table.
            builder.OwnsOne(x => x.Address, AddressBuilder =>
            {
                AddressBuilder.Property(propertyExpression: x => x.Street)
                .HasColumnType("varchar")
                .HasColumnName("Street") // overrides default "Address_Street"
                .HasMaxLength(30);

                AddressBuilder.Property(x => x.City)
                .HasColumnType("varchar")
                .HasColumnName("City") // overrides default "Address_City"
                .HasMaxLength(30);

                AddressBuilder.Property(x => x.BuildingNumber)
                .HasColumnName("BuildingNumber");  // overrides default "Address_BuildingNumber"

            });
        }
    }
}
