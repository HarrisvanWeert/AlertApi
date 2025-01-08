using AlertApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlertApi.Models;

namespace AlertApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AlertController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Alert>>> GetAllAlerts()
        {
            var Alerts = await _context.Alerts.ToListAsync();

            if (Alerts == null)
            {
                return NotFound("No Alerts found");
            }

            return Ok(Alerts);
        }

        [HttpPost]
        public async Task<ActionResult<Alert>> CreateAlert([FromBody] Alert alert)
        {
            // Validate the incoming request
            if (alert == null)
            {
                return BadRequest("Alert data is required.");
            }

            if (string.IsNullOrWhiteSpace(alert.AlertMessage))
            {
                return BadRequest("Alert message cannot be empty.");
            }
            alert.CreatedAt = DateTime.UtcNow;
            alert.UpdatedAt = DateTime.UtcNow;

          
            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllAlerts), new { id = alert.AlertID }, alert);
        }


    }
}
