namespace BackEnd.Data
{
    public class SpeakerImage
    {
        public int SpeakerId { get; set; }
        public Speaker Speaker { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
