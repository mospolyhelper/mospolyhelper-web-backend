namespace Mospolyhelper.Domain.Map.Model.V0_1
{
    public class Pin
    {
        public string Title { get; }

        public string Description { get; }

        public Coordinates Coordinates { get; }

        public Pin(string title, string description, Coordinates coords) {
            this.Title = title;
            this.Description = description;
            this.Coordinates = coords;
        }
    }

}
