namespace WPLBlazor.Data.Models
{
    public class ScheduleData
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public bool IsReadonly { get; set; }
        public int CalendarId { get; set; }
        public int? RecurrenceID { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }

    }
}
