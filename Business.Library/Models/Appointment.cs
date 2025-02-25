using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Library.Models;

public class Appointment : ISoftDeletable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string? Text { get; set; }
    public AppointmentType AppointmentType { get; set; }
    public RecurrentFrequency Repeats { get; set; }
    public bool IsRecurrent => SeriesIdentifier != null;
    public Guid? SeriesIdentifier { get; set; }
    public DateTime RepeatsTo { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOnUtc { get; set; }
}

public enum RecurrentFrequency
{
    None,
    Daily,
    EveryOtherDay,
    WorkingDay,
    Weekly,
    BiWeekly,
    Monthly,
    Quarterly,
    Yearly
}

public enum AppointmentType
{
    Other,
    Meeting,
    Call,
    Holiday,
    Vacation,
    Birthday,
    Anniversary,
    Trip,
    Workshop,
    Interview,
    Conference,
    Appointment
}