using AppointmentScheduler.Services;
using AppointmentScheduler.Utility;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        public IActionResult Index()
        {
            ViewBag.Duration = Helper.GetTimeDropDown();
            ViewBag.DoctorsList = _appointmentService.GetDoctorsList();
            ViewBag.PatientsList = _appointmentService.GetPatientsList();
            return View();
        }
    }
}
