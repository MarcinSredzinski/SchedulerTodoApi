using Business.Library.Models;
using Business.Library.Models.OperationResults;

namespace Business.Library.Repositories;

public interface IAppointmentsRepository
{
    OperationResult AddAppointment(Appointment appointment);
    OperationResult AddSeries(Appointment appointment);
    OperationResult DeleteAppointment(Appointment appointment);
    OperationResult DeleteSeries(Appointment appointment);
    OperationResult UpdateSeries(Appointment appointment);
    OperationResult UpdateAppointment(Appointment appointment);
    OperationResult UpdateAppointment(Appointment originalAppointment, Appointment updatedAppointment);
    Appointment? GetBy(int id);
    IQueryable<Appointment> GetAppointments(DateTime startDate, DateTime endDate);
}