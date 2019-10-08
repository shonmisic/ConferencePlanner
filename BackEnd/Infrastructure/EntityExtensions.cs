using BackEnd.Data;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace BackEnd.Infrastructure
{
    public static class EntityExtensions
    {
        private static readonly object _baseUrlFrontEnd = "https://localhost:44354";

        public static Conference MapConference(this ConferenceDTO.ConferenceRequest conference)
        {
            var newConference = new Conference
            {
                ID = conference.ID,
                Name = conference.Name,
                StartTime = conference.StartTime,
                EndTime = conference.EndTime,
                Sessions = conference.Sessions?.Select(s => s.MapSession()).ToList(),
                Tracks = conference.Tracks?.Select(t => t.MapTrack()).ToList(),
            };

            newConference.ConferenceAttendees = conference.Attendees?
                                                    .Select(a => new ConferenceAttendee
                                                    {
                                                        Conference = newConference,
                                                        ConferenceId = newConference.ID,
                                                        AttendeeId = a.ID,
                                                        Attendee = a.MapAttendee()
                                                    })
                                                    .ToList();

            return newConference;
        }

        public static Attendee MapAttendee(this ConferenceDTO.Attendee attendee) =>
            new Attendee
            {
                ID = attendee.ID,
                UserName = attendee.UserName,
                FirstName = attendee.FirstName,
                LastName = attendee.LastName,
                EmailAddress = attendee.EmailAddress,
            };

        public static Speaker MapSpeaker(this ConferenceDTO.Speaker speaker) =>
            new Speaker
            {
                ID = speaker.ID,
                Bio = speaker.Bio,
                Name = speaker.Name,
                WebSite = speaker.WebSite
            };

        public static Track MapTrack(this ConferenceDTO.Track track) =>
            new Track
            {
                ID = track.ID,
                Name = track.Name,
                ConferenceId = track.ConferenceId
            };

        public static ConferenceDTO.SessionResponse MapSessionResponse(this Session session) =>
            new ConferenceDTO.SessionResponse
            {
                ID = session.ID,
                Title = session.Title,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Tags = session.SessionTags?
                              .Select(st => new ConferenceDTO.Tag
                              {
                                  ID = st.TagId,
                                  Name = st.Tag?.Name
                              })
                               .ToList(),
                Speakers = session.SessionSpeakers?
                                  .Select(ss => new ConferenceDTO.Speaker
                                  {
                                      ID = ss.SpeakerId,
                                      Name = ss.Speaker?.Name
                                  })
                                   .ToList(),
                TrackId = session.TrackId ?? 0,
                Track = new ConferenceDTO.Track
                {
                    ID = session.TrackId ?? 0,
                    Name = session.Track?.Name
                },
                ConferenceId = session.ConferenceId,
                Abstract = session.Abstract,
                Attendees = session.SessionAttendees?
                                   .Select(sa => new ConferenceDTO.Attendee
                                   {
                                       ID = sa.AttendeeId,
                                       UserName = sa.Attendee?.UserName,
                                       FirstName = sa.Attendee?.FirstName,
                                       LastName = sa.Attendee?.LastName,
                                       EmailAddress = sa.Attendee?.EmailAddress
                                   })
                                   .ToList()
            };

        public static ConferenceDTO.SpeakerResponse MapSpeakerResponse(this Speaker speaker) =>
            new ConferenceDTO.SpeakerResponse
            {
                ID = speaker.ID,
                Name = speaker.Name,
                Bio = speaker.Bio,
                WebSite = speaker.WebSite,
                Sessions = speaker.SessionSpeakers?
                    .Select(ss =>
                        new ConferenceDTO.Session
                        {
                            ID = ss.SessionId,
                            Title = ss.Session?.Title,
                            Url = CreateSessionUrl(ss.SessionId)
                        })
                    .ToList()
            };

        public static Speaker MapSpeaker(this ConferenceDTO.SpeakerRequest speaker)
        {
            var mappedSpeaker = new Speaker
            {
                Name = speaker.Name,
                Bio = speaker.Bio,
                WebSite = speaker.WebSite,
            };

            mappedSpeaker.SessionSpeakers =
                speaker.Sessions?.Select(s => new SessionSpeaker
                {
                    Speaker = mappedSpeaker,
                    Session = s.MapSession()
                })
                .ToList();

            mappedSpeaker.SpeakerImages?.Add(new SpeakerImage
            {
                Speaker = mappedSpeaker,
                Image = speaker.Image.MapImage()
            });

            return mappedSpeaker;
        }

        public static Image MapImage(this ConferenceDTO.ImageRequest image) =>
            new Image
            {
                Content = image.Content,
                ImageType = image.ImageType,
                Name = image.Name,
                UploadDate = DateTimeOffset.Now,
            };

        public static ConferenceDTO.AttendeeResponse MapAttendeeResponse(this Attendee attendee) =>
            new ConferenceDTO.AttendeeResponse
            {
                ID = attendee.ID,
                FirstName = attendee.FirstName,
                LastName = attendee.LastName,
                UserName = attendee.UserName,
                EmailAddress = attendee.EmailAddress,
                Sessions = attendee.SessionAttendees?
                    .Select(sa =>
                        new ConferenceDTO.Session
                        {
                            ID = sa.SessionId,
                            Title = sa.Session?.Title,
                            StartTime = sa.Session?.StartTime,
                            EndTime = sa.Session?.EndTime,
                            Url = CreateSessionUrl(sa.SessionId)
                        })
                    .ToList(),
                Images = attendee.AttendeeImages?
                    .Select(ai =>
                        new ConferenceDTO.Image
                        {
                            ID = ai.ImageId,
                            UploadDate = ai.Image?.UploadDate,
                            Content = ai.Image?.Content,
                            Name = ai.Image?.Name,
                            Url = CreateImageUrl(ai.ImageId)
                        })
                    .ToList(),
                Conferences = attendee.ConferenceAttendees?
                    .Select(ca =>
                        new ConferenceDTO.Conference
                        {
                            ID = ca.ConferenceId,
                            EndTime = ca.Conference?.EndTime,
                            Name = ca.Conference?.Name,
                            StartTime = ca.Conference?.StartTime,
                            Url = CreateConferenceUrl(ca.ConferenceId)
                        })
                    .ToList()
            };

        public static void UpdateValuesFrom(this Attendee attendee, ConferenceDTO.Attendee input)
        {
            attendee.EmailAddress = input.EmailAddress;
            attendee.FirstName = input.FirstName;
            attendee.LastName = input.LastName;
        }

        public static ConferenceDTO.ImageResponse MapImageResponse(this Image image) =>
            new ConferenceDTO.ImageResponse
            {
                Content = image.Content,
                ID = image.ID,
                Name = image.Name,
                UploadDate = image.UploadDate,
                ImageType = image.ImageType
            };

        public static ConferenceDTO.TrackResponse MapTrackResponse(this Track track) =>
            new ConferenceDTO.TrackResponse
            {
                ID = track.ID,
                Conference = track.Conference,
                ConferenceId = track.ConferenceId,
                Name = track.Name,
                Sessions = track.Sessions?
                    .Select(s => new ConferenceDTO.Session
                    {
                        ID = s.ID,
                        ConferenceId = s.ConferenceId,
                        Abstract = s.Abstract,
                        EndTime = s.EndTime,
                        StartTime = s.StartTime,
                        Title = s.Title,
                        TrackId = s.TrackId,
                        Url = CreateSessionUrl(s.ID)
                    })
                    .ToList()
            };

        public static ConferenceDTO.ConferenceResponse MapConferenceResponse(this Conference conference) =>
            new ConferenceDTO.ConferenceResponse
            {
                ID = conference.ID,
                Name = conference.Name,
                StartTime = conference.StartTime,
                EndTime = conference.EndTime,
                Url = CreateConferenceUrl(conference.ID),
                Sessions = conference.Sessions?.Select(s => s.MapSession()).ToList(),
                Speakers = conference.Sessions?.SelectMany(s => s.SessionSpeakers).Select(ss => ss.Speaker?.MapSpeaker()).ToList(),
                Tracks = conference.Tracks.Select(t => t.MapTrack()).ToList(),
                Attendees = conference.ConferenceAttendees.Select(ca => ca.Attendee?.MapAttendee()).ToList()
            };

        public static ConferenceDTO.Speaker MapSpeaker(this Speaker speaker) =>
            new ConferenceDTO.Speaker
            {
                Bio = speaker.Bio,
                ID = speaker.ID,
                Name = speaker.Name,
                WebSite = speaker.WebSite
            };

        public static ConferenceDTO.Track MapTrack(this Track track) =>
            new ConferenceDTO.Track
            {
                ID = track.ID,
                Name = track.Name,
                ConferenceId = track.ConferenceId
            };

        public static ConferenceDTO.Attendee MapAttendee(this Attendee attendee) =>
            new ConferenceDTO.Attendee
            {
                EmailAddress = attendee.EmailAddress,
                FirstName = attendee.FirstName,
                ID = attendee.ID,
                LastName = attendee.LastName,
                Url = CreateAttendeeUrl(attendee.ID),
                UserName = attendee.UserName
            };

        public static ConferenceDTO.Session MapSession(this Session session) =>
            new ConferenceDTO.Session
            {
                ID = session.ID,
                Title = session.Title,
                Abstract = session.Abstract,
                ConferenceId = session.ConferenceId,
                EndTime = session.EndTime,
                StartTime = session.StartTime,
                TrackId = session.TrackId,
                Url = CreateSessionUrl(session.ID)
            };

        public static Track MapTrack(this ConferenceDTO.TrackRequest trackRequest) =>
            new Track
            {
                ID = trackRequest.ID,
                ConferenceId = trackRequest.ConferenceId,
                Name = trackRequest.Name,
            };

        public static Session MapSession(this ConferenceDTO.Session session) =>
            new Session
            {
                Title = session.Title,
                ConferenceId = session.ConferenceId,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Abstract = session.Abstract,
                TrackId = session.TrackId
            };

        public static byte[] ToByteArray<T>(this T obj)
        {
            if (obj == null)
            {
                return null;
            }

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);

                return ms.ToArray();
            }
        }

        public static T FromByteArray<T>(this byte[] data)
        {
            if (data == null)
            {
                return default(T);
            }

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream(data))
            {
                return (T) bf.Deserialize(ms);
            }
        }

        private static string CreateAttendeeUrl(int id)
        {
            return $"{_baseUrlFrontEnd}/Attendees/{id}";
        }

        private static string CreateSessionUrl(int id)
        {
            return $"{_baseUrlFrontEnd}/Sessions/{id}";
        }

        private static string CreateConferenceUrl(int id)
        {
            return $"{_baseUrlFrontEnd}/Conferences/{id}";
        }

        private static string CreateImageUrl(int id)
        {
            return $"{_baseUrlFrontEnd}/Images/{id}";
        }
    }
}
