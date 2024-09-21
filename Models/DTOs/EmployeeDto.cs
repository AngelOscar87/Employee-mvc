namespace EMPLEADOS.DTOs
{
    public class EmployeeDto
{
    public long EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? HireDate { get; set; }
    public double Salary { get; set; }
    public long DepartmentId { get; set; }
}
}
