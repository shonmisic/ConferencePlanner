﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceDTO;

namespace FrontEnd.Services
{
    public interface IApiClient
    {
        Task<ICollection<SessionResponse>> GetSessionsAsync();
        Task<SessionResponse> GetSessionAsync(int id);
        Task<ICollection<SpeakerResponse>> GetSpeakersAsync();
        Task<SpeakerResponse> GetSpeakerAsync(int id);
        Task PutSessionAsync(Session session);
        Task<bool> AddAttendeeAsync(Attendee attendee);
        Task<AttendeeResponse> GetAttendeeAsync(string name);
        Task DeleteSessionAsync(int id);
        Task<ICollection<SearchResult>> SearchAsync(string query);
        Task<ICollection<SessionResponse>> GetSessionsByAttendeeAsync(string name);
        Task AddSessionToAttendeeAsync(string name, int sessionId);
        Task RemoveSessionFromAttendeeAsync(string name, int sessionId);
        Task<bool> CheckHealthAsync();
        Task<ICollection<ImageResponse>> GetImagesAsync();
        Task AddImageToAttendeeAsync(string username, ImageRequest imageRequest);
        Task<ICollection<TrackResponse>> GetTracks(int conferenceId);
    }
}
