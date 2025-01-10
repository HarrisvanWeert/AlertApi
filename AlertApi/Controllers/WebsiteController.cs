namespace AlertApi.Controllers
{
    using AlertApi.Data;
    using AlertApi.Dto;
    using AlertApi.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="WebsiteController" />
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WebsiteController : ControllerBase
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteController"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="ApplicationDbContext"/></param>
        public WebsiteController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// The CreateWebsite
        /// </summary>
        /// <param name="website">The website<see cref="Website"/></param>
        /// <returns>The <see cref="Task{ActionResult{Alert}}"/></returns>


        [HttpPost]
        public async Task<ActionResult<Alert>> CreateWebsite([FromBody] WebsiteCreateDto websiteDto)
        {

            if (websiteDto == null)
            {
                return BadRequest("No website found");
            }

            var website = new Website
            {
                WebsiteName = websiteDto.WebsiteName,
                WebsiteUrl = websiteDto.WebsiteUrl,
                Status = websiteDto.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Websites.Add(website);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllWebsites),new {id = website.WebsiteID},website);
        }


        /// <summary>
        /// The GetAllWebsites
        /// </summary>
        /// <returns>The <see cref="Task{ActionResult{List{Website}}}"/></returns>
        [HttpGet]
        public async Task<ActionResult<List<Website>>> GetAllWebsites()
        {
            var websites = await _context.Websites.ToListAsync();

            if (websites == null)
            {
                return NotFound("No websites found");
            }

            return Ok(websites);
        }

        /// <summary>
        /// The GetWebsiteByName
        /// </summary>
        /// <param name="WebsiteName">The WebsiteName<see cref="string"/></param>
        /// <returns>The <see cref="Task{ActionResult{List{Website}}}"/></returns>
        [HttpGet("WebsiteName/{WebsiteName}")]
        public async Task<ActionResult<List<Website>>> GetWebsiteByName(string WebsiteName)
        {

            if (string.IsNullOrWhiteSpace(WebsiteName))
            {
                return BadRequest("Alert type is required.");
            }

            var WebsiteNames = await _context.Websites
                .Where(a => EF.Functions.Like(a.WebsiteName, $"%{WebsiteName}%"))
                .ToListAsync();

            if (!WebsiteNames.Any())
            {
                return NotFound($"No Alerts found for the WebsiteName: {WebsiteName}");
            }

            return Ok(WebsiteNames);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Website>> UpdateWebsite(int id, [FromBody] Website website)
        {
            if (website == null)
            {
                return BadRequest("Website data is required.");
            }

            var existingWebsite = await _context.Websites.FirstOrDefaultAsync(a => a.WebsiteID == id);

            if (existingWebsite == null)
            {
                return NotFound("Website not found.");
            }

            if (!string.IsNullOrWhiteSpace(website.WebsiteName))
            {
                existingWebsite.WebsiteName = website.WebsiteName;
            }

            if (!string.IsNullOrWhiteSpace(website.WebsiteUrl))
            {
                existingWebsite.WebsiteUrl = website.WebsiteUrl;
            }

            if (string.IsNullOrWhiteSpace(website.WebsiteName) && string.IsNullOrWhiteSpace(website.WebsiteUrl))
            {
                return BadRequest("At least one property (WebsiteName or WebsiteUrl) must be provided for the update.");
            }

            existingWebsite.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(existingWebsite);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWebsite(int id)
        {
            var website = await _context.Websites.FirstOrDefaultAsync(a => a.WebsiteID == id);

            if (website == null)
            {
                return NotFound("Website not found.");
            }

            _context.Websites.Remove(website);
            await _context.SaveChangesAsync();

            return Ok(new { website, message = "was deleted" });
        }

    }
}
