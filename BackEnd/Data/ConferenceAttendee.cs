namespace BackEnd.Data
{
    public class ConferenceAttendee
    {
        public int ConferenceId { get; set; }
        public Conference Conference { get; set; }
        public int AttendeeId { get; set; }
        public Attendee Attendee { get; set; }
    }
}
