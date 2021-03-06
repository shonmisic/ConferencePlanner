﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Models;
using FrontEnd.Services;

namespace ConferencePlanner.Tests.Mocks
{
    public class MockApiClient : IApiClient
    {
        public Task<bool> CreateAttendeeAsync(ConferenceDTO.Attendee attendee)
        {
            throw new NotImplementedException();
        }

        public Task AddImageToAttendeeAsync(string username, ImageRequest imageRequest)
        {
            throw new NotImplementedException();
        }

        public Task AddSessionToAttendeeAsync(string name, int sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckHealthAsync()
        {
            throw new NotImplementedException();
        }

        public Task CreateTrackAsync(TrackRequest trackRequest)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSessionAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTrackAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<AttendeeResponse> GetAttendeeAsync(string name)
        {
            return await  Task.FromResult(new AttendeeResponse
            {
                ID = 1,
                EmailAddress = "email@e.a",
                FirstName = "John",
                LastName = "Doe",
                UserName = "username"
            });
        }

        public Task<ConferenceResponse> GetConference(int conferenceId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConferenceResponse>> GetConferencesForFollowingFiveDays()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ImageResponse>> GetImagesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<SessionResponse> GetSessionAsync(int id)
        {
            return await Task.FromResult(new SessionResponse
            {
                ID = 1,
                Title = "title",
                Abstract = "abstract",
                ConferenceId = 1,
                Track = new Track
                {
                    ConferenceId = 1,
                    ID = 1,
                    Name = "track1",
                },
                StartTime = new DateTimeOffset(2019, 8, 8, 18, 0, 0, new TimeSpan(0, 0, 0)),
                EndTime = new DateTimeOffset(2019, 8, 8, 19, 0, 0, new TimeSpan(0, 0, 0))
            });
        }

        public async Task<ICollection<SessionResponse>> GetSessionsAsync(int conferenceId)
        {
            return await Task.FromResult(new List<SessionResponse>
                {
                    new SessionResponse
                    {
                        ID = 1,
                        Title = "title",
                        Abstract = "abstract",
                        ConferenceId = 1,
                        Track = new Track
                        {
                            ConferenceId = 1,
                            ID = 1,
                            Name = "track1",
                        },
                        StartTime = new DateTimeOffset(2019, 8, 8, 18, 0, 0, new TimeSpan(0, 0, 0)),
                        EndTime = new DateTimeOffset(2019, 8, 8, 19, 0, 0, new TimeSpan(0, 0, 0)),
                    },
                    new SessionResponse
                    {
                        ID = 2,
                        Title = "title2",
                        Abstract = "abstract2",
                        ConferenceId = 1,
                        Track = new Track
                        {
                            ConferenceId = 1,
                            ID = 2,
                            Name = "track2",
                        },
                        StartTime = new DateTimeOffset(2019, 08, 09, 18, 0, 0, new TimeSpan(0, 0, 0)),
                        EndTime = new DateTimeOffset(2019, 08, 09, 19, 0, 0, new TimeSpan(0, 0, 0)),
                    },
                }
            );
        }

        public Task<ICollection<SessionResponse>> GetSessionsByAttendeeAsync(string name, int conferenceId)
        {
            return Task.FromResult<ICollection<SessionResponse>>(new List<SessionResponse>
                {
                    new SessionResponse
                    {
                        ID = 1,
                        Title = "title",
                        Abstract = "abstract",
                        ConferenceId = 1,
                        Track = new Track
                        {
                            ConferenceId = 1,
                            ID = 1,
                            Name = "track1",
                        },
                        StartTime = new DateTimeOffset(2019, 8, 8, 18, 0, 0, new TimeSpan(0, 0, 0)),
                        EndTime = new DateTimeOffset(2019, 8, 8, 19, 0, 0, new TimeSpan(0, 0, 0)),
                    },
                    new SessionResponse
                    {
                        ID = 2,
                        Title = "title2",
                        Abstract = "abstract2",
                        ConferenceId = 1,
                        Track = new Track
                        {
                            ConferenceId = 1,
                            ID = 2,
                            Name = "track2",
                        },
                        StartTime = new DateTimeOffset(2019, 08, 09, 18, 0, 0, new TimeSpan(0, 0, 0)),
                        EndTime = new DateTimeOffset(2019, 08, 09, 19, 0, 0, new TimeSpan(0, 0, 0)),
                    },
                }
            );
        }

        public Task<SpeakerResponse> GetSpeakerAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<SpeakerResponse>> GetSpeakersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TrackResponse>> GetTracks(int conferenceId)
        {
            throw new NotImplementedException();
        }

        public Task PutSessionAsync(Session session)
        {
            throw new NotImplementedException();
        }

        public Task RemoveSessionFromAttendeeAsync(string name, int sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<SearchResult>> SearchAsync(string query)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAttendeeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task CreateSpeakerAsync(SpeakerRequest speaker)
        {
            throw new NotImplementedException();
        }

        public Task CreateConferenceAsync(ConferenceRequest conference)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ConferenceResponse>> GetAllConferencesAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteConference(int conferenceId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AttendeeResponse>> GetAllAttendeesAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteSpeakerAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAttendeeAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task PutAttendeeAsync(FrontEnd.Models.Attendee attendee)
        {
            throw new NotImplementedException();
        }

        public Task RemoveConferenceFromAttendeeAsync(string username, int conferenceId)
        {
            throw new NotImplementedException();
        }

        public Task AddConferenceToAttendeeAsync(string userName, int conferenceId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<SessionResponse>> GetSessionsByTrackAsync(int trackId)
        {
            throw new NotImplementedException();
        }
    }
}
