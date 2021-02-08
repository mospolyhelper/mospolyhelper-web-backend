namespace Mospolyhelper.Domain.Map.Model.V0_1
{
    using System.Collections.Generic;

    public class Map
    {
        public long Version { get; }

        public IDictionary<string, IList<Pin>> Addresses { get; }

        public Map(
            long version,
            IDictionary<string, IList<Pin>> addresses
        ) {
            this.Version = version;
            this.Addresses = addresses;
        }
    }

}