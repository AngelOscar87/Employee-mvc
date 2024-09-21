using Microsoft.EntityFrameworkCore;
using EMPLEADOS.Models;

namespace EMPLEADOS.Data
{
    public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Department> Departments { get; set; } 
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración para la tabla Departments
        modelBuilder.Entity<Department>()
            .HasKey(d => d.DepartmentId);

        modelBuilder.Entity<Department>()
            .Property(d => d.DepartmentId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Department>()
            .Property(d => d.DepartmentName)
            .IsRequired();

        // Configuración para la tabla Employees
        modelBuilder.Entity<Employee>()
            .HasKey(e => e.EmployeeId);

        modelBuilder.Entity<Employee>()
            .Property(e => e.EmployeeId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Employee>()
            .Property(e => e.FirstName)
            .IsRequired();

        modelBuilder.Entity<Employee>()
            .Property(e => e.LastName)
            .IsRequired();

        // Relación: un departamento tiene muchos empleados
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId);
    }
}

}
