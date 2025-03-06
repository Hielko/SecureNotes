using System.ComponentModel.DataAnnotations;

namespace LightControl.Models
{
    public class Settings : ISettings
    {

        [Range(0, 36000)]
        [Display(Name = "Duur in minuten")]
        public int DurationMinutes { get; set; } = 10;

        [Range(5, 300)]
        [Display(Name = "RandomMaxOffSeconds")]
        public int RandomMaxOffSeconds { get; set; } = 10;

        [Range(10, 30)]
        [Display(Name = "RandomMaxOnSeconds")]
        public int RandomMaxOnSeconds { get; set; } = 10;

        [Range(2, 6)]
        [Display(Name = "MinTimeOnSeconds")]
        public int MinTimeOnSeconds { get; set; } = 5;

        [Range(2, 10)]
        [Display(Name = "MinTimeOffSeconds")]
        public int MinTimeOffSeconds { get; set; } = 5;

    }
}