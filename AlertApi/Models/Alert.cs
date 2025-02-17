﻿namespace AlertApi.Models
{
    public class Alert
    {
        public int AlertID { get; set; } 
        public string AlertMessage { get; set; }
        public string AlertType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
