namespace Domain.ViewModel
{
    public abstract class PeopleVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateOnly? DeathDate { get; set; }
    }
}
