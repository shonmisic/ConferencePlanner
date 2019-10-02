using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions)
            : base(contextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendee>()
                .HasIndex(a => a.UserName)
                .IsUnique();

            //modelBuilder.Entity<Attendee>()
            //    .HasMany(sa => sa.SessionAttendees)
            //    .WithOne(sa => sa.Attendee)
            //    .HasForeignKey(sa => sa.AttendeeId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Attendee>()
            //    .HasMany(ca => ca.ConferenceAttendees)
            //    .WithOne(ca => ca.Attendee)
            //    .HasForeignKey(ca => ca.AttendeeId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Attendee>()
            //    .HasMany(a => a.AttendeeImages)
            //    .WithOne(ai => ai.Attendee)
            //    .HasForeignKey(ai => ai.AttendeeId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AttendeeImage>()
                .HasKey(ai => new { ai.AttendeeId, ai.ImageId });

            //modelBuilder.Entity<Conference>()
            //    .HasMany(c => c.ConferenceAttendees)
            //    .WithOne(ca => ca.Conference)
            //    .HasForeignKey(ca => ca.ConferenceId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Conference>()
                .HasMany(c => c.Tracks)
                .WithOne(t => t.Conference)
                .HasForeignKey(t => t.ConferenceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Conference>()
                .HasMany(c => c.Sessions)
                .WithOne(s => s.Conference)
                .HasForeignKey(c => c.ConferenceId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ConferenceAttendee>()
                .HasKey(ca => new { ca.ConferenceId, ca.AttendeeId });

            modelBuilder.Entity<ConferenceSpeaker>()
                .HasKey(cs => new { cs.ConferenceId, cs.SpeakerId });

            modelBuilder.Entity<Session>()
                .Ignore(s => s.Duration);

            //modelBuilder.Entity<Session>()
            //    .HasMany(s => s.SessionSpeakers)
            //    .WithOne(ss => ss.Session)
            //    .HasForeignKey(ss => ss.SessionId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Session>()
            //    .HasMany(s => s.SessionAttendees)
            //    .WithOne(ss => ss.Session)
            //    .HasForeignKey(sa => sa.SessionId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Session>()
            //    .HasMany(s => s.SessionTags)
            //    .WithOne(st => st.Session)
            //    .HasForeignKey(st => st.SessionId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SessionAttendee>()
                .HasKey(sa => new { sa.SessionId, sa.AttendeeId });

            modelBuilder.Entity<SessionSpeaker>()
                .HasKey(ss => new { ss.SessionId, ss.SpeakerId });

            modelBuilder.Entity<SessionTag>()
                .HasKey(st => new { st.SessionId, st.TagId });

            //modelBuilder.Entity<Speaker>()
            //    .HasMany(s => s.SessionSpeakers)
            //    .WithOne(ss => ss.Speaker)
            //    .HasForeignKey(ss => ss.SpeakerId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Speaker>()
            //    .HasMany(s => s.SpeakerImages)
            //    .WithOne(si => si.Speaker)
            //    .HasForeignKey(si => si.SpeakerId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SpeakerImage>()
                .HasKey(si => new { si.SpeakerId, si.ImageId });

            //modelBuilder.Entity<Tag>()
            //    .HasMany(s => s.SessionTags)
            //    .WithOne(st => st.Tag)
            //    .HasForeignKey(st => st.TagId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Track>()
                .HasMany(t => t.Sessions)
                .WithOne(s => s.Track)
                .HasForeignKey(t => t.TrackId);

            modelBuilder.Entity<Track>()
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);
        }

        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
