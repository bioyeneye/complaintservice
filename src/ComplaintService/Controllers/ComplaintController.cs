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
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}

//dotnet ef dbcontext scaffold "server=localhost;uid=sa;pwd=Password1@;database=WalletDatabase" Microsoft.EntityFrameworkCore.SqlServer --startup-project WalletSystem --project WalletSystem.DataAccess  --context-dir DbContext  --output-dir Entities