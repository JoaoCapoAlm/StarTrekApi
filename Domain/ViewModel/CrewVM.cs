namespace Domain.ViewModel
{
    public class CrewVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }
        public DateTime? DeathDate { get; set; }
    }
}
