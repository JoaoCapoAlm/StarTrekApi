using Application.Model;

namespace Application.Data.ViewModel
{
    public class MovieVM(short movieId, string originalName, string synopsis, string originalLanguage,
        short time, string imdbId, DateOnly? releaseDate, byte timelineId)
    {
        public short Id { get; set; } = movieId;
        public string OriginalName { get; set; } = originalName;
        public string Synopsis { get; set; } = synopsis;
        public string OriginalLanguage { get; set; } = originalLanguage;
        public DateOnly? ReleaseDate { get; set; } = releaseDate;
        public short Time { get; set; } = time;
        public byte TimelineId { get; set; } = timelineId;
        public string ImdbId { get; set; } = imdbId;
    }
}
