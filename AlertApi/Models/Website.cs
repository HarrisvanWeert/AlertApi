namespace AlertApi.Models
{
    public class Website
    {
        public int WebsiteID { get; set; } 
        public string WebsiteName { get; set; }
        public string WebsiteUrl { get; set; }
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
