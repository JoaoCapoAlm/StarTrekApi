namespace Application.Model
{
    public class Cast
    {
        public Guid CastId { get; set; }
        public string Name { get; set; }
        public DateTime? Birth { get; set; }
        public short? CountryId { get; set; }
        public virtual Country Country { get; set; }

        public Cast(string name, DateTime? birth = null, short? countryId = null)
        {
            CastId = Guid.NewGuid();
            Name = name;
            Birth = birth;
            CountryId = countryId;
        }
    }
}
