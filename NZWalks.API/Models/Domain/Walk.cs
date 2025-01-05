namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInkm { get; set; }
        public string? RegionImageUrl { get; set; }
        public int MyProperty { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }

        // Navigation Property
        public Difficulty Difficulty { get; set; }
        public Region Region { get; set; }
    }
}
