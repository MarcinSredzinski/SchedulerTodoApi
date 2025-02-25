namespace Business.Library.Models
{
    public class Rootobject
    {
        public OpenHoliday[] Holidays { get; set; }
    }
    public class OpenHoliday
    {

        public string Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Type { get; set; }
        public HolidayName[] Name { get; set; }
        public bool Nationwide { get; set; }
        public Subdivision[] Subdivisions { get; set; }
    }
    public class HolidayName
    {
        public string? Language { get; set; }
        public string? Text { get; set; }
    }

    public class Name
    {
        public string Language { get; set; }
        public string Text { get; set; }
    }

    public class Subdivision
    {
        public string Code { get; set; }
        public string ShortName { get; set; }
    }
}
