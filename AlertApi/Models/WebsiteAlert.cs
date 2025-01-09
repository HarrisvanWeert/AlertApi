namespace AlertApi.Models
{
    public class WebsiteAlert
    {
        public int WebsiteAlertID { get; set; } 
        public int WebsiteID { get; set; } 
        public int AlertID { get; set; } 
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Website Website { get; set; }
        public Alert Alert { get; set; }
    }

}
