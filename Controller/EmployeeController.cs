using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMPLEADOS.Data;
using EMPLEADOS.DTOs;
using EMPLEADOS.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace EMPLEADOS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/employee
        [HttpGet]
        [SwaggerOperation(Summary = "Obtiene todos los empleados", Description = "Obtiene una lista de todos los empleados")]
        [SwaggerResponse(200, "Lista de empleados", typeof(IEnumerable<EmployeeDto>))]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var employees = await _context.Employees
                .Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    DateOfBirth = e.DateOfBirth,
                    HireDate = e.HireDate,
                    Salary = (double)e.Salary,
                    DepartmentId = e.DepartmentId
                })
                .ToListAsync();

            return Ok(employees);
        }

        // GET: api/employee/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtiene un empleado por ID", Description = "Obtiene los detalles de un empleado específico mediante su ID")]
        [SwaggerResponse(200, "Detalles del empleado", typeof(EmployeeDto))]
        [SwaggerResponse(404, "Empleado no encontrado")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(long id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeDto = new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth,
                HireDate = employee.HireDate,
                Salary = (double)employee.Salary,
                DepartmentId = employee.DepartmentId
            };

            return Ok(employeeDto);
        }

        // POST: api/employee
        [HttpPost]
        [SwaggerOperation(Summary = "Crea un nuevo empleado", Description = "Crea un nuevo empleado en la base de datos")]
        [SwaggerResponse(201, "Empleado creado con éxito", typeof(EmployeeDto))]
        [Consumes("application/json")] // Define el tipo de contenido permitido
        public async Task<ActionResult<EmployeeDto>> PostEmployee([FromBody] EmployeeDto employeeDto)
        {
            var employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                DateOfBirth = employeeDto.DateOfBirth?.ToUniversalTime() ?? default(DateTime),
                HireDate = employeeDto.HireDate?.ToUniversalTime() ?? default(DateTime),
                Salary = (decimal)employeeDto.Salary,
                DepartmentId = employeeDto.DepartmentId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            employeeDto.EmployeeId = employee.EmployeeId;

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employeeDto);
        }

        // PUT: api/employee/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Actualiza un empleado existente", Description = "Actualiza los detalles de un empleado")]
        [SwaggerResponse(204, "Actualización exitosa")]
        [SwaggerResponse(400, "Solicitud inválida")]
        [Consumes("application/json")] // Define el tipo de contenido permitido
        public async Task<IActionResult> PutEmployee(long id, [FromBody] EmployeeDto employeeDto)
        {
            if (id != employeeDto.EmployeeId)
            {
                return BadRequest();
            }

            var employee = new Employee
            {
                EmployeeId = employeeDto.EmployeeId,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                DateOfBirth = employeeDto.DateOfBirth?.ToUniversalTime() ?? default(DateTime),
                HireDate = employeeDto.HireDate?.ToUniversalTime() ?? default(DateTime),
                Salary = (decimal)employeeDto.Salary,
                DepartmentId = employeeDto.DepartmentId
            };

            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/employee/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Elimina un empleado", Description = "Elimina un empleado de la base de datos mediante su ID")]
        [SwaggerResponse(204, "Empleado eliminado con éxito")]
        [SwaggerResponse(404, "Empleado no encontrado")]
        public async Task<IActionResult> DeleteEmployee(long id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
