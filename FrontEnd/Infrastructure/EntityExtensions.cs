using ConferenceDTO;

namespace FrontEnd.Infrastructure
{
    public static class EntityExtensions
    {
        public static ConferenceResponse MapConferenceResponse(this Conference conference) =>
            new ConferenceResponse
            {
                ID = conference.ID,
                Name = conference.Name,
                StartTime = conference.StartTime,
                EndTime = conference.EndTime,
                Url = conference.Url,
            };
    }
}
