namespace AlertApi.Controllers
{
    using AlertApi.Data;
    using AlertApi.Dto;
    using AlertApi.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="AlertController" />
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AlertController : ControllerBase
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertController"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="ApplicationDbContext"/></param>
        public AlertController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// The GetAllAlerts
        /// </summary>
        /// <returns>The <see cref="Task{ActionResult{List{Alert}}}"/></returns>
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

        /// <summary>
        /// The GetAlert
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <returns>The <see cref="Task{ActionResult{Alert}}"/></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Alert>> GetAlert(int id)
        {
            var alert = await _context.Alerts.FirstOrDefaultAsync(a => a.AlertID == id);

            if (alert == null)
            {
                return NotFound("Alert not found.");
            }

            return Ok(alert);
        }

        /// <summary>
        /// The GetAlertsByAlertType
        /// </summary>
        /// <param name="AlertType">The AlertType<see cref="string"/></param>
        /// <returns>The <see cref="Task{ActionResult{List{Alert}}}"/></returns>
        [HttpGet("AlertType/{AlertType}")]
        public async Task<ActionResult<List<Alert>>> GetAlertsByAlertType(string AlertType)
        {

            if (string.IsNullOrWhiteSpace(AlertType))
            {
                return BadRequest("Alert type is required.");
            }

            var AlertTypes = await _context.Alerts
                .Where(a => EF.Functions.Like(a.AlertType, $"%{AlertType}%"))
                .ToListAsync();

            if (!AlertTypes.Any())
            {
                return NotFound($"No Alerts found for the AlertType: {AlertType}");
            }

            return Ok(AlertTypes);
        }

        /// <summary>
        /// The CreateAlert
        /// </summary>
        /// <param name="alert">The alert<see cref="Alert"/></param>
        /// <returns>The <see cref="Task{ActionResult{Alert}}"/></returns>
        

        /// <summary>
        /// The UpdateAlert
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <param name="alert">The alert<see cref="Alert"/></param>
        /// <returns>The <see cref="Task{ActionResult{Alert}}"/></returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<Alert>> UpdateAlert(int id, [FromBody] Alert alert)
        {
            if (alert == null)
            {
                return BadRequest("Alert data is required.");
            }

            if (string.IsNullOrWhiteSpace(alert.AlertMessage))
            {
                return BadRequest("Alert message cannot be empty.");
            }

            var existingAlert = await _context.Alerts.FirstOrDefaultAsync(a => a.AlertID == id);

            if (existingAlert == null)
            {
                return NotFound("Alert not found.");
            }

            if (!string.IsNullOrWhiteSpace(alert.AlertMessage))
            {
                existingAlert.AlertMessage = alert.AlertMessage;
            }

            existingAlert.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(existingAlert);
        }

        /// <summary>
        /// The DeleteAlert
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAlert(int id)
        {
            var alert = await _context.Alerts.FirstOrDefaultAsync(a => a.AlertID == id);

            if (alert == null)
            {
                return NotFound("Alert not found.");
            }

            _context.Alerts.Remove(alert);
            await _context.SaveChangesAsync();

            return Ok(new { alert, message = "was deleted" });
        }

        [HttpPost]
        public async Task<ActionResult<Alert>> CreateAlert([FromBody] AlertCreateDto alertDto)
        {
            if (alertDto == null)
            {
                return BadRequest("Alert data is required.");
            }

            if (string.IsNullOrWhiteSpace(alertDto.AlertMessage))
            {
                return BadRequest("Alert message cannot be empty.");
            }

           
            var alert = new Alert
            {
                AlertMessage = alertDto.AlertMessage,
                AlertType = alertDto.AlertType,
                StartTime = alertDto.StartTime,
                EndTime = alertDto.EndTime,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllAlerts), new { id = alert.AlertID }, alert);
        }




    }
}
