using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ComplaintService.BusinessDomain.Services;
using ComplaintService.DataAccess.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComplaintService.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]", Name = nameof(ComplaintController))]
    public class ComplaintController : Controller
    {
        private readonly IComplaintService _service;

        public ComplaintController(IComplaintService service)
        {
            _service = service;
        }
        
        [Route("Count")]
        public IActionResult GetCount(int page, int size, string whereCondition = null, string orderByExpression = null)
        {
            try
            {
                var filter = ComplaintFilter.Deserialize(whereCondition);
                filter.ComplainBy = GetCurrentUserId();
                var responseData = _service.GetCount(page, size, filter, orderByExpression);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut]
        public IActionResult Update(ComplaintModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _service.UpdateComplaint(model, GetCurrentUserId());
                    return Ok(_service.GetById(model.Id));
                }
                return BadRequest(ModelState);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete]
        public IActionResult Delete(ComplaintModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _service.Delete(model.Id);
                    return Ok();
                }
                return BadRequest(ModelState);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [Route("GetDetails/{id}")]
        public IActionResult GetDetail(string id)
        {
            try
            {
                var responseData = _service.GetDetailsById(id);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Post(ComplaintModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.ComplainBy = GetCurrentUserId();
                    _service.Create(model, GetCurrentUserId());
                    return Ok(model);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [NonAction]
        public string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}