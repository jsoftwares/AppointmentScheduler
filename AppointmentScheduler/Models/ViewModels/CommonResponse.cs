namespace AppointmentScheduler.Models.ViewModels
{
    public class CommonResponse<T>  //<T> means its a generic type; some API may return int, string, object from DB etc
    {
        public int status { get; set; }
        public string message { get; set; }
        public T dataenum { get; set; }
    }
}
