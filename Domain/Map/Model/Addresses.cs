namespace Mospolyhelper.Domain.Map.Model
{
    public class Addresses
    {
        public Pin[] Campuses { get; }
        public Pin[] Gyms { get; }
        public Pin[] Hostels { get; }

        public Addresses(Pin[] campuses, Pin[] gyms, Pin[] hostels) {
            this.Campuses = campuses;
            this.Gyms = gyms;
            this.Hostels = hostels;
        }
    }

}