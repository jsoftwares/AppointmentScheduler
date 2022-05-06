using AppointmentScheduler.Models.ViewModels;

namespace AppointmentScheduler.Services
{
    public interface IAppointmentService
    {
        public List<DoctorVM> GetDoctorsList();
        public List<PatientVM> GetPatientsList();
        public Task<int> AddUpdate(AppointmentVM model);    //returns an in, type of Task so we can implement d async verion
        public List<AppointmentVM> DoctorAppointmentsById(string doctorId);
        public List<AppointmentVM> PatientAppointmentsById(string patientId);

    }
}
