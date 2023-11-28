using Basket.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasketController : ControllerBase
    {

        [HttpPost]
        public IActionResult Save(StockSaveDto request)
        {


            return  Ok(request);
        }
    }
}
