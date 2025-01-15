namespace AlertApi.Controllers
{
    using AlertApi.Data;
    using AlertApi.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="AlertWebsiteController" />
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AlertWebsiteController : ControllerBase
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertWebsiteController"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="ApplicationDbContext"/></param>
        public AlertWebsiteController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// The GetAllWebsiteAlerts
        /// </summary>
        /// <returns>The <see cref="Task{ActionResult{List{WebsiteAlert}}}"/></returns>
        [HttpGet]
        public async Task<ActionResult<List<WebsiteAlert>>> GetAllWebsiteAlerts()
        {
            var websiteAlerts = await _context.WebsiteAlerts.ToListAsync();

            if (websiteAlerts == null)
            {
                return NotFound("No website alerts found.");
            }

            return Ok(websiteAlerts);
        }

        /// <summary>
        /// The AssignAlertToWebsites
        /// </summary>
        /// <param name="alertId">The alertId<see cref="int"/></param>
        /// <param name="websiteIds">The websiteIds<see cref="List{int}"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
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

        /// <summary>
        /// The UnassignAlertFromWebsite
        /// </summary>
        /// <param name="alertId">The alertId<see cref="int"/></param>
        /// <param name="websiteId">The websiteId<see cref="int"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpDelete("{alertId}/unassign-from-website/{websiteId}")]
        public async Task<IActionResult> UnassignAlertFromWebsite(int alertId, int websiteId)
        {

            var websiteAlert = await _context.WebsiteAlerts
                .FirstOrDefaultAsync(wa => wa.AlertID == alertId && wa.WebsiteID == websiteId);

            if (websiteAlert == null)
            {
                return NotFound("The specified alert-website relationship does not exist.");
            }

            _context.WebsiteAlerts.Remove(websiteAlert);

            await _context.SaveChangesAsync();
            return Ok("Alert unassigned from website successfully.");
        }

        [HttpGet("{websiteId}")]
        public async Task<ActionResult<List<Alert>>> GetAlertsForWebsite(int websiteId)
        {
            var alerts = await _context.WebsiteAlerts
                .Where(wa => wa.WebsiteID == websiteId)
                .Include(wa => wa.Alert) 
                .Select(wa => wa.Alert) 
                .ToListAsync();

            if (alerts == null || alerts.Count == 0)
            {
                return NotFound("No alerts found for this website.");
            }

            return Ok(alerts);
        }
    }
}
