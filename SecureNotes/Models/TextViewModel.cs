namespace SecureNotes.Models
{
    public class TextViewModel
    {
        public string? Text { get; set; }
        public string? Filename { get; set; }

        public List<string> Filenames { get; set; } = new();
    }
}