using AppointmentScheduler.Models.ViewModels;

namespace AppointmentScheduler.Services
{
    public interface IAppointmentService
    {
        public List<DoctorVM> GetDoctorsList();
        public List<PatientVM> GetPatientsList();
    }
}
