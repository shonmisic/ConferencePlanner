using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceDTO;

namespace FrontEnd.Services
{
    public interface IApiClient
    {
        Task<ICollection<SessionResponse>> GetSessionsAsync(int conferenceId);
        Task<SessionResponse> GetSessionAsync(int id);
        Task<ICollection<SpeakerResponse>> GetSpeakersAsync();
        Task<SpeakerResponse> GetSpeakerAsync(int id);
        Task PutSessionAsync(Session session);
        Task<bool> CreateAttendeeAsync(Attendee attendee);
        Task<AttendeeResponse> GetAttendeeAsync(string name);
        Task DeleteSessionAsync(int id);
        Task<ICollection<SearchResult>> SearchAsync(string query);
        Task<ICollection<AttendeeResponse>> GetAllAttendeesAsync();
        Task CreateConferenceAsync(ConferenceRequest conference);
        Task CreateSpeakerAsync(SpeakerRequest speaker);
        Task DeleteSpeakerAsync(int id);
        Task<ICollection<SessionResponse>> GetSessionsByAttendeeAsync(string name, int conferenceId);
        Task<ICollection<SessionResponse>> GetSessionsByTrackAsync(int trackId);
        Task<ICollection<ConferenceResponse>> GetAllConferencesAsync();
        Task AddSessionToAttendeeAsync(string name, int sessionId);
        Task RemoveSessionFromAttendeeAsync(string name, int sessionId);
        Task DeleteAttendeeAsync(string username);
        Task<bool> CheckHealthAsync();
        Task<ICollection<ImageResponse>> GetImagesAsync();
        Task CreateTrackAsync(TrackRequest trackRequest);
        Task PutAttendeeAsync(Models.Attendee attendee);
        Task AddImageToAttendeeAsync(string username, ImageRequest imageRequest);
        Task<ICollection<TrackResponse>> GetTracks(int conferenceId);
        Task<IEnumerable<ConferenceResponse>> GetConferencesForFollowingFiveDays();
        Task<ConferenceResponse> GetConference(int conferenceId);
        Task DeleteTrackAsync(int id);
        Task DeleteConference(int conferenceId);
        Task RemoveConferenceFromAttendeeAsync(string username, int conferenceId);
        Task AddConferenceToAttendeeAsync(string userName, int conferenceId);
    }
}
