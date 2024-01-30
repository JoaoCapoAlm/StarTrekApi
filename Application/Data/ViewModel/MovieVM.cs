namespace Application.Data.ViewModel
{
    public class MovieVM
    {
        public short Id { get; set; }
        public string OriginalName { get; set; }
        public string Synopsis { get; set; }
        public string OriginalLanguage { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public short Time { get; set; }
        public string ImdbId { get; set; }

        public MovieVM(short movieId,
            string originalName,
            string synopsis,
            string originalLanguage,
            short time,
            string imdbId,
            DateOnly? releaseDate)
        {
            Id = movieId;
            OriginalName = originalName;
            Synopsis = synopsis;
            OriginalLanguage = originalLanguage;
            Time = time;
            ImdbId = imdbId;
            ReleaseDate = releaseDate.Value;
        }
    }
}
