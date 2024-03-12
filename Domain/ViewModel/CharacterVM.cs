namespace Domain.ViewModel
{
    public class CharacterVM : PeopleVM
    {
        public DateOnly? DateBirth { get; set; }
        public SpeciesVM Species { get; set; }
    }
}
