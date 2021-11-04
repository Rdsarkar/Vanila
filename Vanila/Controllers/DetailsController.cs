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
    public class SelfClass2
    {
        public decimal? Id { get; set; }
    }

    public class SelfOutput
    {
        public decimal? Id { get; set; }
        public string Name { get; set; }
        public string DName { get; set; }
    }

    public class SelfOutput2
    {
        public decimal? Id { get; set; }
        public string Name { get; set; }
    }



    [Route("api/[controller]")]
    [ApiController]
    public class DetailsController : ControllerBase
    {
        private readonly ModelContext _context;

        public DetailsController(ModelContext context)
        {
            _context = context;
        }

        [HttpGet("AllDetails")]
        public async Task<ActionResult<ResponseDto>> GetAll()
        {

            List<SelfOutput> selfOutputs =
               await (from d in _context.Details

                      from de in _context.Departments
                                           .Where(i => i.DId == d.DId)
                      select new SelfOutput
                      {
                          Id = d.Id,
                          Name = d.Name,
                          DName = de.DName
                      }).OrderBy(i => i.Id).ToListAsync();
            if (selfOutputs == null)
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
                Message = "Joining Done",
                Success = true,
                Payload = selfOutputs
            });
        }

        [HttpPost("DetailsWithDepartmentNameSingle")]
        public async Task<ActionResult<ResponseDto>> GetDname([FromBody] SelfClass2 input)
        {

            if (input.Id == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Data is not found in the database!",
                    Success = false,
                    Payload = null
                });
            }
            List<SelfOutput> selfOutputs =
                await (from d in _context.Details
                                            .Where(i => i.Id == input.Id)
                       from de in _context.Departments
                                            .Where(i => i.DId == d.DId)
                       select new SelfOutput
                       {
                           Id = d.Id,
                           Name = d.Name,
                           DName = de.DName
                       }).OrderBy(i => i.Id).ToListAsync();
            if (selfOutputs == null)
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
                Message = "Joining Done",
                Success = true,
                Payload = selfOutputs
            });
        }


        [HttpPost("UpdateName")]
        public async Task<ActionResult<ResponseDto>> UpdateName([FromBody] SelfOutput2 input)
        {
            if (input.Id == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Please FillUp ID field!",
                    Success = false,
                    Payload = null
                });
            }

            var details = await _context.Details.Where(i => i.Id == input.Id).FirstOrDefaultAsync();
            if (details == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "Data is not found",
                    Success = false,
                    Payload = null
                });
            }

            details.Name = input.Name;


            _context.Details.Update(details);

            bool isSaved = await _context.SaveChangesAsync() > 0;
            if (isSaved == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Message = "Internal Server Error!!",
                    Success = false,
                    Payload = null
                }); ;
            }
            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "True",
                Success = true,
                Payload = details.Name
            }); ;
        }

        // DELETE: api/Details/5
        [HttpPost("DeleteData")]
        public async Task<ActionResult<ResponseDto>> DeleteDetails([FromBody] SelfClass2 input)
        {
            if (input.Id == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Please fill up the DID field",
                    Success = false,
                    Payload = null
                });
            }
            var detail = await _context.Details.Where(i => i.DId == input.Id).FirstOrDefaultAsync();
            if (detail == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "Data is not found",
                    Success = false,
                    Payload = null
                });
            }

            _context.Details.Remove(detail);
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

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetDetails()
        {
            List<Detail> details = await _context.Details
                                                     .OrderBy(i => i.Id)
                                                     .ToListAsync();
            if (details.Count <= 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "Data pacche nah",
                    Success = false,
                    Payload = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "Data pacche ",
                Success = true,
                Payload = details
            });
        }


        [HttpPost("UpdateAllDetails")]
        public async Task<ActionResult<ResponseDto>> UpdateAll([FromBody] Detail input)
        {
            if (input.Id == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Please FillUp ID field!",
                    Success = false,
                    Payload = null
                });
            }

            if (input.Name == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Please FillUp Name field!",
                    Success = false,
                    Payload = null
                });
            }

            if (input.DId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Please FillUp Depertment ID field!",
                    Success = false,
                    Payload = null
                });
            }

            var details = await _context.Details.Where(i => i.Id == input.Id).FirstOrDefaultAsync();
            if (details == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "Data is not found",
                    Success = false,
                    Payload = null
                });
            }

            details.DId = input.DId;
            details.Name = input.Name;

            _context.Details.Update(details);

            bool isSaved = await _context.SaveChangesAsync() > 0;
            if (isSaved == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Message = "Internal Server Error!!",
                    Success = false,
                    Payload = null
                }); ;
            }
            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "True",
                Success = true,
                Payload = details
            }); ;
        }
        private bool DetailExists(decimal? id)
        {
            return _context.Details.Any(e => e.Id == id);
        }
    }
}
