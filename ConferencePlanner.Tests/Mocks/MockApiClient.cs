﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Services;

namespace ConferencePlanner.Tests.Mocks
{
    public class MockApiClient : IApiClient
    {
        public Task<bool> AddAttendeeAsync(Attendee attendee)
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

        public Task DeleteSessionAsync(int id)
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

        public async Task<List<SessionResponse>> GetSessionsAsync()
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

        public Task<List<SessionResponse>> GetSessionsByAttendeeAsync(string name)
        {
            return Task.FromResult(new List<SessionResponse>
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

        public Task<List<SpeakerResponse>> GetSpeakersAsync()
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

        public Task<List<SearchResult>> SearchAsync(string query)
        {
            throw new NotImplementedException();
        }
    }
}
