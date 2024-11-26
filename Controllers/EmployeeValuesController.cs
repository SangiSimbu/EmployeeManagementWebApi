using EmpMgmtApp;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManagementWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeValuesController : ControllerBase
    {
        private readonly EmployeeADO employeeADO;
        public EmployeeValuesController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            employeeADO = new EmployeeADO(connectionString);
        }

        // GET: api/<EmployeeValuesController>
        [HttpGet("/ViewEmployee")]
        public IActionResult GetEmployees()
        {
            var employees = employeeADO.ViewEmployees();
            return Ok(employees);
        }

        // GET api/<EmployeeValuesController>/5
        [HttpGet("/ViewEmployeeByID/{id}")]
        public IActionResult GetEmployee(int id)
        {
            var employee = employeeADO.GetEmployeeByID(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        // POST api/<EmployeeValuesController>
        [HttpPost("/AddEmployee")]
        public IActionResult AddEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }
            employeeADO.AddEmployee(employee);
            return Ok($"Employee {employee.Name} Added Successfully");
        }

        // PUT api/<EmployeeValuesController>/5
        [HttpPut("/UpdateEmployee/{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] Employee employee)
        {
            
            if (employee == null)
            {
                return BadRequest();
            }
            employee.EmpId = id;
            employeeADO.UpdateEmployee(employee);
            return Ok($"Employee with EmpID {id} Updated Successfully");
        }

        // DELETE api/<EmployeeValuesController>/5
        [HttpDelete("/DeleteEmployee/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            //var existingEmployee = employeeADO.GetEmployeeById(id);
            employeeADO.DeleteEmployee(id);
            return Ok($"Employee with EmpID {id} Deleted Successfully");
        }
    }
}
