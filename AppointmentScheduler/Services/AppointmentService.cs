﻿using AppointmentScheduler.Models;
using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Utility;

namespace AppointmentScheduler.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;

        public AppointmentService(ApplicationDbContext db)
        {
            _db = db;  
        }

        public async Task<int> AddUpdate(AppointmentVM model)
        {
            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));

            if (model !=null && model.Id > 0)
            {
                //Update
                return 1;
            }
            else
            {
                //Create
                Appointment appointment = new Appointment()
                {
                    //Title = model.Title,
                    //Description = model.Description,
                    //StartDate = startDate,
                    //EndDate = endDate,
                    //Duration = model.Duration,
                    //DoctorId = model.DoctorId,
                    //PatientId = model.PatientId,
                    //AdminId = model.AdminId,
                    //isDoctorApproved = false
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = model.Duration,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    IsDoctorApproved = false,
                    AdminId = model.AdminId
                };
                _db.Appointments.Add(appointment);
                await _db.SaveChangesAsync();
                
                return 2;
            }
            
        }

        /**We get all d appointments where the DoctorId equals d doctorId passed as argument, we then convert d result to a List,
         * then we user projecttions (Select) since we want to return a List of AppointmentVMs; for every element on the returned
         * List of filtered appointments from DB, we create a new object of type AppointmentVM. If we do not do this then return
         type will be the Appointment model but we need to convert from this to AppointmentVM 
        */
        public List<AppointmentVM> DoctorAppointmentsById(string doctorId)
        {
            return _db.Appointments.Where(x => x.DoctorId == doctorId).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                DoctorId = c.DoctorId,
                Title = c.Title,
                Description = c.Description,
                Duration = c.Duration,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                IsDoctorApproved=c.IsDoctorApproved
            }).ToList();
        }

        public List<DoctorVM> GetDoctorsList()
        {
            var doctors = (from user in _db.Users
                           join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                           join roles in _db.Roles.Where( x => x.Name == Helper.Doctor) on userRoles.RoleId equals roles.Id
                           select new DoctorVM { Id = user.Id, Name = user.Name }
                           ).ToList();
            return doctors;
        }

        public List<PatientVM> GetPatientsList()
        {
            var patients = (from user in _db.Users
                           join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                           join roles in _db.Roles.Where(x => x.Name == Helper.Patient) on userRoles.RoleId equals roles.Id
                           select new PatientVM { Id = user.Id, Name = user.Name }
                           ).ToList();
            return patients;
        }

        public List<AppointmentVM> PatientAppointmentsById(string patientId)
        {
            return _db.Appointments.Where(x => x.PatientId == patientId).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                DoctorId = c.DoctorId,
                Title = c.Title,
                Description = c.Description,
                Duration = c.Duration,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                IsDoctorApproved = c.IsDoctorApproved
            }).ToList();
        }
    }
}
