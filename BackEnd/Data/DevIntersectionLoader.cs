using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BackEnd
{
    public class DevIntersectionLoader : DataLoader
    {
        public override async Task<Conference> LoadDataAsync(string conferenceName, Stream fileStream)
        {
            var reader = new JsonTextReader(new StreamReader(fileStream));

            var conference = new Conference { Name = conferenceName };

            var speakerNames = new Dictionary<string, Speaker>();
            var tracks = new Dictionary<string, Track>();

            var doc = await JArray.LoadAsync(reader);

            foreach (var item in doc)
            {
                var theseSpeakers = new List<Speaker>();
                foreach (var thisSpeakerName in item["speakerNames"])
                {
                    if (!speakerNames.ContainsKey(thisSpeakerName.Value<string>()))
                    {
                        var thisSpeaker = new Speaker { Name = thisSpeakerName.Value<string>() };
                        conference.Speakers.Add(thisSpeaker);
                        speakerNames.Add(thisSpeakerName.Value<string>(), thisSpeaker);
                        Console.WriteLine(thisSpeakerName.Value<string>());
                    }
                    theseSpeakers.Add(speakerNames[thisSpeakerName.Value<string>()]);
                }

                var theseTracks = new List<Track>();
                foreach (var thisTrackName in item["trackNames"])
                {
                    if (!tracks.ContainsKey(thisTrackName.Value<string>()))
                    {
                        var thisTrack = new Track { Name = thisTrackName.Value<string>(), Conference = conference };
                        conference.Tracks.Add(thisTrack);
                        tracks.Add(thisTrackName.Value<string>(), thisTrack);
                    }
                    theseTracks.Add(tracks[thisTrackName.Value<string>()]);
                }

                var session = new Session
                {
                    Conference = conference,
                    Title = item["title"].Value<string>(),
                    StartTime = item["startTime"].Value<DateTime>(),
                    EndTime = item["endTime"].Value<DateTime>(),
                    Track = theseTracks[0],
                    Abstract = item["abstract"].Value<string>()
                };

                session.SessionSpeakers = theseSpeakers.Select(s => new SessionSpeaker
                {
                    Session = session,
                    Speaker = s
                }).ToList();

                conference.Sessions.Add(session);
            }

            return conference;
        }
    }
}

