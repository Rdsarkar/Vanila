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
    public class SelfClass222
    {
        public string J { get; set; }
    }
    public class SelfClass22
    {
        public decimal? DIdAlvi { get; set; }
        //public List<ABCD_CLASS> GTY { get; set; }
    }

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

        [HttpGet("AllDetails2")]
        public async Task<ActionResult<ResponseDto>> AllDetails2()
        {

            var selfOutputs = await _context.Details.ToListAsync();
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
            var detail = await _context.Details.Where(i => i.Id == input.Id).FirstOrDefaultAsync();
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

        [HttpPost("CreateDetails")]
        public async Task<ActionResult<ResponseDto>> CreateDetails([FromBody] Detail input)
        {
            if (input.Id == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Please Insert Id Field!",
                    Success = false,
                    Payload = null
                });
            }
            if (input.Name == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "please Fill UP Name Field!",
                    Success = false,
                    Payload = null
                });
            }
            if (input.DId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "please Fill UP DID Field!",
                    Success = false,
                    Payload = null
                });
            }

            Detail details = await _context.Details.Where(i => i.Id == input.Id).FirstOrDefaultAsync();
            if (details != null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "please Fill UP DID Field!",
                    Success = false,
                    Payload = null
                });
            }
            _context.Details.Add(input);
            bool isSaved = await _context.SaveChangesAsync() > 0;
            if (isSaved == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Message = "Server Error so cant be Insert data",
                    Success = false,
                    Payload = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "Data Inserted",
                Success = true,
                Payload = null
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
        [HttpPost("DeleteDptWithStudent")]
        public async Task<ActionResult<ResponseDto>> DeleteDptWithStudent([FromBody] SelfClass22 input)
        {
            if (input.DIdAlvi == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Please FillUp ID field!",
                    Success = false,
                    Payload = null
                });
            }

            Department department = await _context.Departments.Where(i => i.DId == input.DIdAlvi).FirstOrDefaultAsync();
            List<Detail> detail = await _context.Details.Where(i => i.DId == input.DIdAlvi).ToListAsync();
            //List<Detail> detail = await _context.Details.Take(10).ToListAsync();
            //List<Detail> detail = await _context.Details.Skip(10).Take(10).ToListAsync();

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (detail.Count > 0)
                {
                    foreach (var item in detail)
                    {
                        _context.Details.Remove(item);
                        await _context.SaveChangesAsync();
                    }
                }
                if (department != null)
                {
                    _context.Departments.Remove(department);
                    await _context.SaveChangesAsync();
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Message = "Operation failed. Something went wrong. Please try again later",
                    Success = false,
                    Payload = new
                    {
                        ex.StackTrace,
                        ex.InnerException,
                        ex.Message,
                        ex.Data,
                        ex.Source
                    }
                });
            }



            //bool isSaved = await _context.SaveChangesAsync() > 0;
            //if (isSaved == false)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
            //    {
            //        Message = "Internal Server Error!!",
            //        Success = false,
            //        Payload = null
            //    }); ;
            //}
            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "True",
                Success = true,
                Payload = null
            }); ;
        }
        [HttpPost("MultiColumnSearch")]
        public async Task<ActionResult<ResponseDto>> MultiColumnSearch([FromBody] SelfClass222 input)
        {
            if (input.J == null || input.J == "")
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Please FillUp ID field!",
                    Success = false,
                    Payload = null
                });
            }
            var fff =
                await (from de in _context.Details
                       join d in _context.Departments
                        on de.DId equals d.DId
                       where de.Name.Trim().ToLower().Contains(input.J.Trim().ToLower()) ||
                       de.Id.ToString().Trim().ToLower().Contains(input.J.Trim().ToLower()) ||
                       de.DId.ToString().Trim().ToLower().Contains(input.J.Trim().ToLower()) ||
                       d.DName.Trim().ToLower().Contains(input.J.Trim().ToLower())
                       select new Detail
                       {
                           Id = de.Id,
                           Name = de.Name,
                           DId = de.DId
                       }).ToListAsync();

            //List<Detail> detail = await _context.Details.Where(i => i.Name.Trim().ToLower().Contains(input.J.Trim().ToLower()) || i.Id.ToString().Trim().ToLower().Contains(input.J.Trim().ToLower()) || i.DId.ToString().Trim().ToLower().Contains(input.J.Trim().ToLower())).ToListAsync();


            if (fff.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "false",
                    Success = false,
                    Payload = null
                }); ;
            }




            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "True",
                Success = true,
                Payload = fff
            });
        }
        private bool DetailExists(decimal? id)
        {
            return _context.Details.Any(e => e.Id == id);
        }
    }
}
