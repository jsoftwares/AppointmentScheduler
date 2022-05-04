namespace AppointmentScheduler.Models.ViewModels
{
    public class AppointmentVM
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public bool isDoctorApproved { get; set; }
        public string AdminId { get; set; }

        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string AdminName { get; set; }
        public bool isForClient { get; set; }
    }
}
