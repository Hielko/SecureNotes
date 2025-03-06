namespace LightControl.Models
{
    public interface ISettings
    {
        int DurationMinutes { get; set; }
        int MinTimeOffSeconds { get; set; }
        int MinTimeOnSeconds { get; set; }
        int RandomMaxOffSeconds { get; set; }
        int RandomMaxOnSeconds { get; set; }
    }
}