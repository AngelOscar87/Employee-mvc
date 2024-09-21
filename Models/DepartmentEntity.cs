namespace EMPLEADOS.Models
{
    public class Department
    {
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        // Relación uno a muchos con empleados
        public ICollection<Employee> Employees { get; set; }
    }
}
