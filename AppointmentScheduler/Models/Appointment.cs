namespace AppointmentScheduler.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string DoctorId { get; set; }
        public  Guid PatientId { get; set; }
        public bool isDoctorApproved { get; set; }
        public string AdminId { get; set; }

    }
}
