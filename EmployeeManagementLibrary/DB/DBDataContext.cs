﻿namespace EmployeeManagementLibrary.DB;

public class DBDataContext : DbContext
{
    public DBDataContext(DbContextOptions<DBDataContext> options):base(options) { }
    public DbSet<DepartmentModel> Departments { get; set; }

    public DbSet<EmployeeModel> Employees { get; set; }
    public DbSet<UserModel> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DepartmentModel>().ToTable("Departments");
        modelBuilder.Entity<EmployeeModel>().ToTable("Employees");
        modelBuilder.Entity<UserModel>().ToTable("Users");
    }
    public void EnsureDatabaseCreated()
    {
        this.Database.EnsureCreated();
    }
}


