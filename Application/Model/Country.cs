namespace Application.Model
{
    public class Country
    {
        public short CountryId { get; set; }
        public string ResourceName { get; set; }

        public virtual ICollection<Crew> Crews { get; set; }
    }
}
