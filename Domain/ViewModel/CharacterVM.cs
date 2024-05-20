namespace Domain.ViewModel
{
    public class CharacterVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public short? YearBirth { get; set; }
        public byte? MonthBirth { get; set; }
        public byte? DayBirth { get; set; }
        public short? YearDeath { get; set; }
        public byte? MonthDeath { get; set; }
        public byte? DayDeath { get; set; }
        public SpeciesVM Species { get; set; }
    }
}
