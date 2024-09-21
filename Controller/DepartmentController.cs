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
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/department
        [HttpGet]
        [SwaggerOperation(Summary = "Obtiene todos los departamentos", Description = "Obtiene una lista de todos los departamentos en la base de datos")]
        [SwaggerResponse(200, "Lista de departamentos", typeof(IEnumerable<DepartmentDto>))]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        {
            var departments = await _context.Departments
                .Select(d => new DepartmentDto
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName
                })
                .ToListAsync();

            return Ok(departments);
        }

        // GET: api/department/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtiene un departamento por ID", Description = "Obtiene los detalles de un departamento específico mediante su ID")]
        [SwaggerResponse(200, "Detalles del departamento", typeof(DepartmentDto))]
        [SwaggerResponse(404, "Departamento no encontrado")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(long id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            var departmentDto = new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName
            };

            return Ok(departmentDto);
        }

        // POST: api/department
        [HttpPost]
        [SwaggerOperation(Summary = "Crea un nuevo departamento", Description = "Crea un nuevo departamento en la base de datos")]
        [SwaggerResponse(201, "Departamento creado con éxito", typeof(DepartmentDto))]
        [Consumes("application/json")] // Especifica el tipo de contenido permitido
        public async Task<ActionResult<DepartmentDto>> PostDepartment([FromBody] DepartmentDto departmentDto)
        {
            var department = new Department
            {
                DepartmentName = departmentDto.DepartmentName
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            departmentDto.DepartmentId = department.DepartmentId;

            return CreatedAtAction(nameof(GetDepartment), new { id = department.DepartmentId }, departmentDto);
        }

        // PUT: api/department/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Actualiza un departamento existente", Description = "Actualiza el nombre de un departamento específico")]
        [SwaggerResponse(204, "Actualización exitosa")]
        [SwaggerResponse(400, "Solicitud inválida")]
        [Consumes("application/json")] // Especifica el tipo de contenido permitido
        public async Task<IActionResult> PutDepartment(long id, [FromBody] DepartmentDto departmentDto)
        {
            if (id != departmentDto.DepartmentId)
            {
                return BadRequest();
            }

            var department = new Department
            {
                DepartmentId = departmentDto.DepartmentId,
                DepartmentName = departmentDto.DepartmentName
            };

            _context.Entry(department).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/department/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Elimina un departamento", Description = "Elimina un departamento de la base de datos mediante su ID")]
        [SwaggerResponse(204, "Departamento eliminado con éxito")]
        [SwaggerResponse(404, "Departamento no encontrado")]
        public async Task<IActionResult> DeleteDepartment(long id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
