namespace Mospolyhelper.Domain.Map.Model.V0_1
{
    public class Coordinates
    {
        public double Lat { get; }

        public double Lng { get; }

        public Coordinates(double lat, double lng) {
            this.Lat = lat;
            this.Lng = lng;
        }
    }

}
