namespace LightControl.Models
{
    public class InfoModel
    {
        public DateTime? EndTime { get; set; } = null;
        public int? MinutesLeft { get; set; }
        public bool? OnOffStatus { get; set; }
        public string? LastUpdateText { get; set; }
        private string? EndTimeStr => (HasEndTime) ? $"{EndTime:G}" : "No time set";
        public bool HasEndTime => EndTime != null && EndTime != DateTime.MinValue;
        public string? Summary()
        {
            if (HasEndTime) return
                $"{EndTimeStr}, Minutes left: {MinutesLeft}, State: {OnOffStatus}, Text: {LastUpdateText} ";
            else return "Stopped";
        }
    }
}
