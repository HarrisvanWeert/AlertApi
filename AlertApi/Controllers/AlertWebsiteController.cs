using AlertApi.Data;
using AlertApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlertApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertWebsiteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AlertWebsiteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("{alertId}/assign-to-websites")]
        public async Task<IActionResult> AssignAlertToWebsites(int alertId, [FromBody] List<int> websiteIds)
        {
            var alert = await _context.Alerts.FindAsync(alertId);
            if (alert == null)
            {
                return NotFound("Alert not found.");
            }

            foreach (var websiteId in websiteIds)
            {
                var website = await _context.Websites.FindAsync(websiteId);
                if (website == null)
                {
                    return NotFound($"Website with ID {websiteId} not found.");
                }

                // Avoid duplicate assignments
                if (!_context.WebsiteAlerts.Any(aw => aw.AlertID == alertId && aw.WebsiteID == websiteId))
                {
                    _context.WebsiteAlerts.Add(new WebsiteAlert
                    {
                        AlertID = alertId,
                        WebsiteID = websiteId,
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Alert assigned to specified websites.");
        }



    }
}
