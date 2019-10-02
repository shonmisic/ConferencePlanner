namespace BackEnd.Data
{
    public class ConferenceSpeaker
    {
        public int ConferenceId { get; set; }
        public Conference Conference { get; set; }
        public int SpeakerId { get; set; }
        public Speaker Speaker { get; set; }
    }
}
