using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace assignment4.Models;

public partial class NorthwindContext : DbContext
{
    public NorthwindContext()
    {
    }

    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Shipper> Shippers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=northwindDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("employees");

            entity.HasIndex(e => e.LastName, "last_name");

            entity.HasIndex(e => e.PostalCode, "postal_code");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Address)
                .HasMaxLength(60)
                .HasColumnName("address");
            entity.Property(e => e.BirthDate)
                .HasColumnType("datetime")
                .HasColumnName("birth_date");
            entity.Property(e => e.City)
                .HasMaxLength(15)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(15)
                .HasColumnName("country");
            entity.Property(e => e.Extension)
                .HasMaxLength(4)
                .HasColumnName("extension");
            entity.Property(e => e.FirstName)
                .HasMaxLength(10)
                .HasColumnName("first_name");
            entity.Property(e => e.HireDate)
                .HasColumnType("datetime")
                .HasColumnName("hire_date");
            entity.Property(e => e.HomePhone)
                .HasMaxLength(24)
                .HasColumnName("home_phone");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("last_name");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
            entity.Property(e => e.PhotoPath)
                .HasMaxLength(255)
                .HasColumnName("photo_path");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .HasColumnName("postal_code");
            entity.Property(e => e.Region)
                .HasMaxLength(15)
                .HasColumnName("region");
            entity.Property(e => e.ReportsTo).HasColumnName("reports_to");
            entity.Property(e => e.Title)
                .HasMaxLength(30)
                .HasColumnName("title");
            entity.Property(e => e.TitleOfCourtesy)
                .HasMaxLength(25)
                .HasColumnName("title_of_courtesy");

            entity.HasOne(d => d.ReportsToNavigation).WithMany(p => p.InverseReportsToNavigation)
                .HasForeignKey(d => d.ReportsTo)
                .HasConstraintName("FK_employees_employees");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");

            entity.HasIndex(e => e.CustomerId, "customer_id");

            entity.HasIndex(e => e.CustomerId, "customers_orders");

            entity.HasIndex(e => e.EmployeeId, "employee_id");

            entity.HasIndex(e => e.EmployeeId, "employees_orders");

            entity.HasIndex(e => e.OrderDate, "order_date");

            entity.HasIndex(e => e.ShipPostalCode, "ship_postal_code");

            entity.HasIndex(e => e.ShippedDate, "shipped_date");

            entity.HasIndex(e => e.ShipVia, "shippers_orders");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("order_id");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("customer_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Freight)
                .HasDefaultValue(0m)
                .HasColumnType("money")
                .HasColumnName("freight");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.RequiredDate)
                .HasColumnType("datetime")
                .HasColumnName("required_date");
            entity.Property(e => e.ShipAddress)
                .HasMaxLength(60)
                .HasColumnName("ship_address");
            entity.Property(e => e.ShipCity)
                .HasMaxLength(15)
                .HasColumnName("ship_city");
            entity.Property(e => e.ShipCountry)
                .HasMaxLength(15)
                .HasColumnName("ship_country");
            entity.Property(e => e.ShipName)
                .HasMaxLength(40)
                .HasColumnName("ship_name");
            entity.Property(e => e.ShipPostalCode)
                .HasMaxLength(10)
                .HasColumnName("ship_postal_code");
            entity.Property(e => e.ShipRegion)
                .HasMaxLength(15)
                .HasColumnName("ship_region");
            entity.Property(e => e.ShipVia).HasColumnName("ship_via");
            entity.Property(e => e.ShippedDate)
                .HasColumnType("datetime")
                .HasColumnName("shipped_date");

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_orders_employees");

            entity.HasOne(d => d.ShipViaNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShipVia)
                .HasConstraintName("FK_orders_shippers");
        });

        modelBuilder.Entity<Shipper>(entity =>
        {
            entity.ToTable("shippers");

            entity.Property(e => e.ShipperId).HasColumnName("shipper_id");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(40)
                .HasColumnName("company_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(24)
                .HasColumnName("phone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    public DbSet<Employee> employees { get; set; }
}
