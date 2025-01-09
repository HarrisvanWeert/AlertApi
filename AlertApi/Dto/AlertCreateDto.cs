namespace AlertApi.Dto
{
    public class AlertCreateDto
    {
        public string AlertMessage { get; set; }

        public string AlertType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
