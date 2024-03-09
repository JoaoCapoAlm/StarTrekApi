namespace Domain.ViewModel
{
    public class CrewVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Country { get; set; }
        public DateOnly? DeathDate { get; set; }
    }
}
