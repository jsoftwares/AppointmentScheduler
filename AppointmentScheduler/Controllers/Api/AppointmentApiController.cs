using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Services;
using AppointmentScheduler.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppointmentScheduler.Controllers.Api
{
    /** bcos this will not be a generic Controller for an MVC like we have had so far, but an API endpoint, we need to give the
     * Controller a route.
     * Then we use the attribute [ApiController] to define that this is an API endpoint
     * When working with this API, we'd also need to know that the userId and role of the logged in user are. To get these data in 
     * this Controller, we can use HttpContextAccessor that has HttpContext Object which has the generic user that we have used
     * earlier in the application aswell. We have to inject this here; as usual we first add it as service in our Program.cs.
     * Then we can now access the loginUserId using the NameIdentifier Claims type and Role for the logged in user role
     * ClaimTypes are built-in when we are using Identity & once the user logs in this are populated.
     * This will populate the user ID and Role where ever we need it inside our API endpoints
     */
    [Route("api/Appointment")]
    public class AppointmentApiController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string loginUserId;
        private readonly string role;
        public AppointmentApiController(IAppointmentService appointmentService, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        [HttpPost]
        [Route("SaveCalendarData")]
        public IActionResult SaveCalendarData(AppointmentVM data)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status = _appointmentService.AddUpdate(data).Result;
                if (commonResponse.status == 1)
                {
                    commonResponse.message = Helper.appointmentUpdated;
                }
                if (commonResponse.status == 2)
                {
                    commonResponse.message = Helper.appointmentAdded;
                }
            }
            catch (Exception e)
            {

                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }
    }
}
