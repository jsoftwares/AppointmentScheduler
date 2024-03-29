﻿using AppointmentScheduler.Models;
using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace AppointmentScheduler.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;

        public AppointmentService(ApplicationDbContext db)
        {
            _db = db;  
        }

        public async Task<int> AddUpdate(AppointmentVM model)
        {
            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));
            var patient = _db.Users.FirstOrDefault( u => u.Id == model.PatientId);
            var doctor = _db.Users.FirstOrDefault( u => u.Id == model.DoctorId);

            if (model !=null && model.Id > 0)
            {
                //Update
                var appointment = _db.Appointments.FirstOrDefault(x => x.Id == model.Id);
                if (appointment == null)
                {
                    return 0;
                }

                appointment.Title = model.Title;
                appointment.Description = model.Description;
                appointment.StartDate = startDate;
                appointment.EndDate = endDate;
                appointment.Duration = model.Duration;
                appointment.DoctorId = model.DoctorId;
                appointment.PatientId = model.PatientId;
                appointment.IsDoctorApproved = false;
                appointment.AdminId = model.AdminId;
                
                await _db.SaveChangesAsync();

                return 1;
            }
            else
            {
                //Create
                Appointment appointment = new Appointment()
                {
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
                await _emailSender.SendEmailAsync(patient.Email,$"{model.Title} Appointment Created",
                    $"<h4>Dear {patient.Name},</h4><p>Your appointment with ${doctor.Name} has been created and pending status.</p><p>Thank you</p><p><strong>Appointment Scheduler Admin.</strong></p>");
                await _emailSender.SendEmailAsync(doctor.Email, $"{model.Title} Appointment Created", 
                    $"<h4>Dear {doctor.Name},</h4><p>Your appointment with ${patient.Name} has been created and waiting your confirmation.</p><p>Thank you</p><p><strong>Appointment Scheduler Admin.</strong></p>");
                return 2;
            }
            
        }

        public async Task<int> ConfirmAppointment(int id)
        {
            var appointment = _db.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                appointment.IsDoctorApproved = true;
                return await _db.SaveChangesAsync();    //returns the number of record updated
            }
            return 0;
        }

        public async Task<int> Delete(int id)
        {
            var appointment = _db.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                _db.Appointments.Remove(appointment);
                return await _db.SaveChangesAsync();
            }
            return 0;
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

        public AppointmentVM GetById(int id)
        {
            return _db.Appointments.Where(x => x.Id == id).ToList().Select( c => new AppointmentVM()
            {
                Id = c.Id,                
                Title = c.Title,
                Description = c.Description,
                Duration = c.Duration,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                IsDoctorApproved = c.IsDoctorApproved,
                PatientId = c.PatientId,
                DoctorId = c.DoctorId,
                PatientName = _db.Users.Where(x => x.Id == c.PatientId).Select(x => x.Name).FirstOrDefault(),
                DoctorName = _db.Users.Where( x => x.Id == c.DoctorId).Select( x => x.Name).FirstOrDefault(),

            }).SingleOrDefault();
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
