using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using backend.Database;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SealOutController : ControllerBase
    {
        ILogger<SealOutController> _logger;
        DatabaseContext Context;

        public IWebHostEnvironment Env { get; }

        public SealOutController(
            IWebHostEnvironment Env,
            ILogger<SealOutController> logger, DatabaseContext context)
        {
            this.Env = Env;
            _logger = logger;
            Context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = Context.Truck.ToList();
                return Ok(new { result = result, message = "request successfully" });
            }
            catch (Exception error)
            {
                _logger.LogError($"Log GetProducts: {error}");
                return StatusCode(500, new { result = "", message = error });
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = Context.Truck.SingleOrDefault(p => p.TruckId == id);

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(new { result = result, message = "request successfully" });
            }
            catch (Exception error)
            {
                _logger.LogError($"Log Get: {error}");
                return StatusCode(500, new { result = "", message = error });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SealOutTodo model)
        {
            try
            {
                var sealout = new SealOut{
                    sealToTal=model.sealToTal,
                    TruckId = model.TruckId,
                    TruckName=model.TruckName,
                    CreatedBy=model.CreatedBy,
                    UpdatedBy=model.UpdatedBy
                };
                Context.SealOut.Add(sealout);
                Context.SaveChanges();

                return Ok(new { result = model, success = true, message = "เพิ่มข้อมูล  เรียบร้อยแล้ว" });
            }
            catch (Exception error)
            {
                _logger.LogError($"Log Create Truck: {error}");
                return StatusCode(500, new { result = "", message = error });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>
        EditProduct([FromForm] Truck data, int id)
        {
            try
            {
                var product = Context.Truck.SingleOrDefault(p => p.TruckId == id);

                if (product == null)
                {
                    return NotFound();
                }

                Context.Truck.Update(product);
                Context.SaveChanges();

                return Ok(new { result = "", message = "update product successfully" });
            }
            catch (Exception error)
            {
                _logger.LogError($"Log UpdateProduct: {error}");
                return StatusCode(500, new { result = "", message = error });
            }
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var product = Context.Truck.SingleOrDefault(p => p.TruckId == id);

                if (product == null)
                {
                    return NotFound();
                }

                Context.Truck.Remove(product);
                Context.SaveChanges();

                return Ok(new { result = "", message = "delete product sucessfully" });
            }
            catch (Exception error)
            {
                _logger.LogError($"Log DeleteProduct: {error}");
                return StatusCode(500, new { result = "", message = error });
            }
        }
    }
}