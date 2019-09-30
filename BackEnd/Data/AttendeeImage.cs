namespace BackEnd.Data
{
    public class AttendeeImage
    {
        public int AttendeeId { get; set; }
        public Attendee Attendee { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
