using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Data
{
    public class SessionizeLoader : DataLoader
    {
        public override async Task<Conference> LoadDataAsync(string conferenceName, Stream fileStream)
        {
            //var blah = new RootObject().rooms[0].sessions[0].speakers[0].name;

            var addedSpeakers = new Dictionary<string, Speaker>();
            var addedTracks = new Dictionary<string, Track>();
            var addedTags = new Dictionary<string, Tag>();

            var array = await JToken.LoadAsync(new JsonTextReader(new StreamReader(fileStream)));
            var conference = new Conference { Name = conferenceName };

            var root = array.ToObject<List<RootObject>>();

            foreach (var date in root)
            {
                foreach (var room in date.rooms)
                {
                    if (!addedTracks.ContainsKey(room.name))
                    {
                        var thisTrack = new Track { Name = room.name };
                        conference.Tracks.Add(thisTrack);
                        addedTracks.Add(thisTrack.Name, thisTrack);
                    }

                    foreach (var thisSession in room.sessions)
                    {
                        foreach (var speaker in thisSession.speakers)
                        {
                            if (!addedSpeakers.ContainsKey(speaker.name))
                            {
                                var thisSpeaker = new Speaker { Name = speaker.name };
                                conference.Speakers.Add(thisSpeaker);
                                addedSpeakers.Add(thisSpeaker.Name, thisSpeaker);
                            }
                        }

                        foreach (var category in thisSession.categories)
                        {
                            if (!addedTags.ContainsKey(category.name))
                            {
                                var thisTag = new Tag { Name = category.name };
                                addedTags.Add(thisTag.Name, thisTag);
                            }
                        }

                        var session = new Session
                        {
                            Conference = conference,
                            Title = thisSession.title,
                            StartTime = thisSession.startsAt,
                            EndTime = thisSession.endsAt,
                            Track = addedTracks[room.name],
                            Abstract = thisSession.description,
                        };
                        session.SessionTags = addedTags.Select(t => new SessionTag
                        {
                            Session = session,
                            Tag = t.Value
                        }).ToList();

                        session.SessionSpeakers = addedSpeakers.Select(s => new SessionSpeaker
                        {
                            Session = session,
                            Speaker = s.Value
                        }).ToList();

                        conference.Sessions.Add(session);
                    }
                }
            }

            return conference;
        }

        private class RootObject
        {
            public DateTime date { get; set; }
            public List<Room> rooms { get; set; }
            public List<TimeSlot> timeSlots { get; set; }
        }

        private class ImportSpeaker
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        private class Category
        {
            public int id { get; set; }
            public string name { get; set; }
            public List<object> categoryItems { get; set; }
            public int sort { get; set; }
        }

        private class ImportSession
        {
            public int id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public DateTime startsAt { get; set; }
            public DateTime endsAt { get; set; }
            public bool isServiceSession { get; set; }
            public bool isPlenumSession { get; set; }
            public List<ImportSpeaker> speakers { get; set; }
            public List<Category> categories { get; set; }
            public int roomId { get; set; }
            public string room { get; set; }
        }

        private class Room
        {
            public int id { get; set; }
            public string name { get; set; }
            public List<ImportSession> sessions { get; set; }
            public bool hasOnlyPlenumSessions { get; set; }
        }

        private class TimeSlot
        {
            public string slotStart { get; set; }
            public List<Room> rooms { get; set; }
        }
    }
}