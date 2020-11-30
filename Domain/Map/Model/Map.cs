namespace Mospolyhelper.Domain.Map.Model
{
    public class Map
    {
        public long Version { get; }

        public Addresses Addresses { get; }

        public Map(
            long version,
            Addresses addresses
        ) {
            this.Version = version;
            this.Addresses = addresses;
        }
    }

}