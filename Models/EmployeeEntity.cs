namespace EMPLEADOS.Models {
    public class Employee
    {
        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }

        // Relación con Department
        public long DepartmentId { get; set; }
        public Department Department { get; set; }
    }

}
