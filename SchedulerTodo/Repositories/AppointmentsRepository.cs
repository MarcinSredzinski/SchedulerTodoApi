using Business.Library.Models;
using Business.Library.Models.OperationResults;
using Business.Library.Repositories;
using Microsoft.IdentityModel.Tokens;
using SchedulerTodo.DB;

namespace SchedulerTodo.Repositories;

public class AppointmentsRepository(SqlServerDbContext dbContext, ILogger<AppointmentsRepository> logger) : IAppointmentsRepository
{
    public OperationResult AddAppointment(Appointment appointment)
    {
        if (appointment.Text.IsNullOrEmpty())
            return new OperationResult(false, "Cannot add an empty appointment");

        dbContext.Appointments.Add(appointment);
        var affected = dbContext.SaveChanges();
        return affected > 0 ? new OperationSuccessfulResult() : new OperationResult(false, "Appointment not added. Please try again.");
    }

    public OperationResult AddSeries(Appointment appointment)
    {
        if (appointment.Text.IsNullOrEmpty())
            return new OperationResult(false, "Cannot add an empty appointment");
        if (appointment.Repeats == RecurrentFrequency.None)
            return new OperationResult(false, "Cannot add an non recurring appointment as series");
        appointment.SeriesIdentifier = Guid.NewGuid();
        var appointments = GenerateSeriesOfAppointments(appointment);
        dbContext.Appointments.AddRange(appointments);
        dbContext.SaveChanges();
        return new OperationSuccessfulResult();
    }

    private static void CreateNewOccuringAppointmentAndAddToList(Appointment appointment, 
                        ref List<Appointment> appointments,
                        int days = 0,
                        int months = 0,
                        int years = 0)
    {
        if (days > 0)
        {
            appointments.Add(new Appointment
            {
                Text = appointment.Text,
                AppointmentType = appointment.AppointmentType,
                Start = appointment.Start.AddDays(days),
                End = appointment.End.AddDays(days),
                Repeats = appointment.Repeats,
                SeriesIdentifier = appointment.SeriesIdentifier
            });
        }
        else if (months > 0)
        {
            appointments.Add(new Appointment
            {
                Text = appointment.Text,
                AppointmentType = appointment.AppointmentType,
                Start = appointment.Start.AddMonths(months),
                End = appointment.End.AddMonths(months),
                Repeats = appointment.Repeats,
                SeriesIdentifier = appointment.SeriesIdentifier
            });
        }
        else if (years > 0)
        {
            appointments.Add(new Appointment
            {
                Text = appointment.Text,
                AppointmentType = appointment.AppointmentType,
                Start = appointment.Start.AddYears(years),
                End = appointment.End.AddYears(years),
                Repeats = appointment.Repeats,
                SeriesIdentifier = appointment.SeriesIdentifier
            });
        }

    }

    private static List<Appointment> GenerateSeriesOfAppointments(Appointment appointment)
    {
        List<Appointment> appointments = [];

        switch (appointment.Repeats)
        {
            case RecurrentFrequency.Daily:
                for (var i = 0; i < (appointment.RepeatsTo - appointment.Start).Days; i++)
                {
                   CreateNewOccuringAppointmentAndAddToList(appointment, ref appointments, i);
                }
                break;
            case RecurrentFrequency.EveryOtherDay:
                for (var i = 0; i < (appointment.RepeatsTo - appointment.Start).Days; i += 2)
                {
                    CreateNewOccuringAppointmentAndAddToList(appointment, ref appointments, i);
                }
                break;
            case RecurrentFrequency.WorkingDay:
                for (var i = 0; i < (appointment.RepeatsTo - appointment.Start).Days; i += 1)
                {
                    if (appointment.Start.AddDays(i).DayOfWeek == DayOfWeek.Saturday || appointment.Start.AddDays(i).DayOfWeek == DayOfWeek.Sunday)
                        continue;
                    CreateNewOccuringAppointmentAndAddToList(appointment, ref appointments, i);
                }
                break;
            case RecurrentFrequency.Weekly:
                for (var i = 0; i < (appointment.RepeatsTo - appointment.Start).Days; i += 7)
                {
                    CreateNewOccuringAppointmentAndAddToList(appointment, ref appointments, i);
                }
                break;
            case RecurrentFrequency.BiWeekly:
                for (var i = 0; i < (appointment.RepeatsTo - appointment.Start).Days; i += 14)
                {
                    CreateNewOccuringAppointmentAndAddToList(appointment, ref appointments, i);
                }
                break;
            case RecurrentFrequency.Monthly:
                {
                    var previousAppointment = appointment;
                    while (previousAppointment.Start < appointment.RepeatsTo)
                    {
                        CreateNewOccuringAppointmentAndAddToList(appointment, ref appointments, 0, 1);                       
                        previousAppointment = appointments.Last();
                    }
                }
                break;
            case RecurrentFrequency.Quarterly:
                {
                    var previousAppointment = appointment;
                    while (previousAppointment.Start < appointment.RepeatsTo)
                    {
                        CreateNewOccuringAppointmentAndAddToList(appointment, ref appointments, 0, 3);
                        previousAppointment = appointments.Last();
                    }
                }
                break;
            case RecurrentFrequency.Yearly:
                {
                    var previousAppointment = appointment;
                    while (previousAppointment.Start < appointment.RepeatsTo)
                    {
                        CreateNewOccuringAppointmentAndAddToList(appointment, ref appointments, 0, 0, 1);
                        previousAppointment = appointments.Last();
                    }
                }
                break;
            default:
                break;
        }

        return appointments;
    }

    public OperationResult DeleteAppointment(Appointment appointment)
    {
        if (appointment.Text.IsNullOrEmpty())
            return new OperationResult(false, "Cannot remove an empty appointment");

        dbContext.Appointments.Remove(appointment);
        var affected = dbContext.SaveChanges();
        return affected > 0 ? new OperationSuccessfulResult() : new OperationResult(false, "Appointment not deleted. Please try again.");
    }

    public OperationResult UpdateSeries(Appointment appointment)
    {
        var upcommingEvents = dbContext.Appointments.Where(s => s.SeriesIdentifier == appointment.SeriesIdentifier && s.Start > appointment.Start);
        var previousEntry = appointment;

        foreach (var e in upcommingEvents)
        {
            e.Text = appointment.Text;
            e.AppointmentType = appointment.AppointmentType;

            if (e.Repeats != appointment.Repeats)
            {
                e.Repeats = appointment.Repeats;
                switch (e.Repeats)
                {
                    case RecurrentFrequency.Daily:
                        e.Start = previousEntry.Start.AddDays(1);
                        e.End = previousEntry.End.AddDays(1);
                        break;
                    case RecurrentFrequency.EveryOtherDay:
                        e.Start = previousEntry.Start.AddDays(2);
                        e.End = previousEntry.End.AddDays(2);
                        break;
                    case RecurrentFrequency.WorkingDay:
                        {
                            if (previousEntry.Start.AddDays(1).DayOfWeek == DayOfWeek.Saturday || previousEntry.Start.AddDays(1).DayOfWeek == DayOfWeek.Sunday)
                            {
                                continue;
                            }
                            e.Start = previousEntry.Start.AddDays(1);
                            e.End = previousEntry.End.AddDays(1);
                        }
                        break;
                    case RecurrentFrequency.Weekly:
                        e.Start = previousEntry.Start.AddDays(7);
                        e.End = previousEntry.End.AddDays(7);
                        break;
                    case RecurrentFrequency.BiWeekly:
                        e.Start = previousEntry.Start.AddDays(14);
                        e.End = previousEntry.End.AddDays(14);
                        break;
                    case RecurrentFrequency.Monthly:
                        e.Start = previousEntry.Start.AddMonths(1);
                        e.End = previousEntry.End.AddMonths(1);
                        break;
                    case RecurrentFrequency.Quarterly:
                        e.Start = previousEntry.Start.AddMonths(3);
                        e.End = previousEntry.End.AddMonths(3);
                        break;
                    case RecurrentFrequency.Yearly:
                        e.Start = previousEntry.Start.AddYears(1);
                        e.End = previousEntry.End.AddYears(1);
                        break;
                }
            }

            previousEntry = e;
            dbContext.Appointments.Update(e);
        }
        dbContext.SaveChanges();
        return new OperationSuccessfulResult();
    }

    public OperationResult DeleteSeries(Appointment appointment)
    {
        var upcomingEvents = dbContext.Appointments.Where(s => s.SeriesIdentifier == appointment.SeriesIdentifier && s.Start >= appointment.Start);
        foreach (var e in upcomingEvents)
        {
            dbContext.Appointments.Remove(e);
        }
        dbContext.SaveChanges();
        return new OperationSuccessfulResult();
    }

    public OperationResult UpdateAppointment(Appointment appointment)
    {
        dbContext.Appointments.Update(appointment);
        var affected =  dbContext.SaveChanges();
        return affected>0 ? new OperationSuccessfulResult() : new OperationResult(false, "Appointment not updated. Please try again.");

    }

    public OperationResult UpdateAppointment(Appointment originalAppointment, Appointment updatedAppointment)
    {
        var found = dbContext.Appointments
            .FirstOrDefault(a => (a.Start == originalAppointment.Start && a.Text == originalAppointment.Text && a.End == originalAppointment.End));
        if (found != null)
        {
            found.Text = updatedAppointment.Text;
            found.AppointmentType = updatedAppointment.AppointmentType;
            found.Start = updatedAppointment.Start;
            found.End = updatedAppointment.End;
            found.Repeats = updatedAppointment.Repeats;
            dbContext.SaveChanges();
            return new OperationSuccessfulResult();
        }
        const string message = "Appointment not found or not coming from the database. Your changes will not be saved.";
        logger.LogInformation(message);
        return new OperationResult(false, message);
    }

    public IQueryable<Appointment> GetAppointments(DateTime startDate, DateTime endDate)
    {
        var appointments = dbContext.Appointments.Where(a => a.End >= startDate && a.Start <= endDate);
        return appointments;
    }

    protected List<Appointment> GetAll() => [.. dbContext.Appointments];
    

    public Appointment? GetBy(int id)
    {
        return dbContext.Appointments.FirstOrDefault(a => a.Id == id);
    }
}