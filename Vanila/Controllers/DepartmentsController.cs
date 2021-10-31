using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vanila.Models;
using Vanila.DTOs;

namespace Vanila
{
    public class SelfClass1 
    {
        public decimal? DId { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ModelContext _context;

        public DepartmentsController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetDepartments()
        {
            List<Department>departments = await _context.Departments
                                                            .OrderBy(e =>e.DId)
                                                            .ToListAsync();
            if (departments.Count <= 0) 
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto 
                {
                    Message="Data is not found in the database!",
                    Success=false,
                    Payload=null
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "Data Shown",
                Success = true,
                Payload = departments
            });
        }

        // GET: api/Departments/5
        [HttpPost("SingleDepartID")]
        public async Task<ActionResult<ResponseDto>> GetDepartment([FromBody] SelfClass1 input)
        {
            if (input.DId == 0) 
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message="Please fill up the DID field",
                    Success = false,
                    Payload = null
                });
            }

            var department = await _context.Departments.Where(i => i.DId == input.DId).FirstOrDefaultAsync();

            if (department == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "Data is not found",
                    Success = false,
                    Payload = null
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "Data is found",
                Success = true,
                Payload = department
            }); 
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("UpdateData")]
        public async Task<IActionResult> PutDepartment([FromBody] Department input)
        {
            if (input.DId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Please fill up the DID field",
                    Success = false,
                    Payload = null
                });
            }
            if (input.DName == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Please fill up the DName field",
                    Success = false,
                    Payload = null
                });
            }

            var department = await _context.Departments.Where(i => i.DId == input.DId).FirstOrDefaultAsync();
            if (department == null) 
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "Data is not found",
                    Success = false,
                    Payload = null
                });
            }

            department.DName = input.DName;
            _context.Departments.Update(department);

            bool isSaved = await _context.SaveChangesAsync() > 0;

            if (isSaved == false) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Message = "Data is not updated because of Internal Server Error!",
                    Success = false,
                    Payload = null
                });
            }



            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "Data Found",
                Success = true,
                Payload = null
            });
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertNewData")]
        public async Task<ActionResult<ResponseDto>> PostDepartment([FromBody]Department input)
        {
            if (input.DId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Please fill up the DID field",
                    Success = false,
                    Payload = null
                });
            }
            if (input.DName == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Please fill up the DName field",
                    Success = false,
                    Payload = null
                });
            }

            var department = await _context.Departments.Where(i => i.DId == input.DId).FirstOrDefaultAsync();
            if (department != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseDto
                {
                    Message = "Data is already there",
                    Success = false,
                    Payload = null
                });
            }

            _context.Departments.Add(input);
            bool isSaved = await _context.SaveChangesAsync() > 0;

            if (isSaved == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Message = "Data can't inserted because of Internal server Error",
                    Success = false,
                    Payload = null
                });
            }



            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "Data Insterted",
                Success = true,
                Payload = null
            });
        }

        // DELETE: api/Departments/5
        [HttpPost("DeleteData")]
        public async Task<ActionResult> DeleteDepartment([FromBody] SelfClass1 input)
        {
            if (input.DId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Please fill up the DID field",
                    Success = false,
                    Payload = null
                });
            }
            var department = await _context.Departments.Where(i => i.DId == input.DId).FirstOrDefaultAsync();
            if (department == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "Data is not found",
                    Success = false,
                    Payload = null
                });
            }

            _context.Departments.Remove(department);
            bool isSaved = await _context.SaveChangesAsync() > 0;

            if (isSaved == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Message = "Data can't Delete because of Internal server Error",
                    Success = false,
                    Payload = null
                });
            }



            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "Data Deleted",
                Success = true,
                Payload = null
            });
        }

        private bool DepartmentExists(decimal? id)
        {
            return _context.Departments.Any(e => e.DId == id);
        }
    }
}
