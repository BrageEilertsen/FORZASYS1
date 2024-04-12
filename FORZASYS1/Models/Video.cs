namespace FORZASYS1.Models
{
    public class Video
    {
        public string Id { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public string? ThumbnailUrl { get; set; } // URL of the video thumbnail image
        public string? Url { get; set; } // URL of the video page or video file
        // Add other properties as needed
        public string _id { get; set; }
    }
}

