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

            modelBuilder.Entity<AttendeeImage>()
                .HasKey(ai => new { ai.AttendeeId, ai.ImageId });

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

            modelBuilder.Entity<Session>()
                .Ignore(s => s.Duration);

            modelBuilder.Entity<SessionAttendee>()
                .HasKey(sa => new { sa.SessionId, sa.AttendeeId });

            modelBuilder.Entity<SessionSpeaker>()
                .HasKey(ss => new { ss.SessionId, ss.SpeakerId });

            modelBuilder.Entity<SessionTag>()
                .HasKey(st => new { st.SessionId, st.TagId });

            modelBuilder.Entity<SpeakerImage>()
                .HasKey(si => new { si.SpeakerId, si.ImageId });

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
