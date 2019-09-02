﻿// <auto-generated />
using System;
using BackEnd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BackEnd.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190902135602_PostgreSQL")]
    partial class PostgreSQL
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("BackEnd.Data.Attendee", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("ID");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Attendees");
                });

            modelBuilder.Entity("BackEnd.Data.Conference", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("ID");

                    b.ToTable("Conferences");
                });

            modelBuilder.Entity("BackEnd.Data.ConferenceAttendee", b =>
                {
                    b.Property<int>("ConferenceId");

                    b.Property<int>("AttendeeId");

                    b.HasKey("ConferenceId", "AttendeeId");

                    b.HasIndex("AttendeeId");

                    b.ToTable("ConferenceAttendee");
                });

            modelBuilder.Entity("BackEnd.Data.Image", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AttendeeId");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<string>("ImageType")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTimeOffset>("UploadDate");

                    b.HasKey("ID");

                    b.HasIndex("AttendeeId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("BackEnd.Data.Session", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Abstract")
                        .HasMaxLength(4000);

                    b.Property<int>("ConferenceId");

                    b.Property<DateTimeOffset?>("EndTime");

                    b.Property<DateTimeOffset?>("StartTime");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<int?>("TrackId");

                    b.HasKey("ID");

                    b.HasIndex("ConferenceId");

                    b.HasIndex("TrackId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("BackEnd.Data.SessionAttendee", b =>
                {
                    b.Property<int>("SessionId");

                    b.Property<int>("AttendeeId");

                    b.HasKey("SessionId", "AttendeeId");

                    b.HasIndex("AttendeeId");

                    b.ToTable("SessionAttendee");
                });

            modelBuilder.Entity("BackEnd.Data.SessionSpeaker", b =>
                {
                    b.Property<int>("SessionId");

                    b.Property<int>("SpeakerId");

                    b.HasKey("SessionId", "SpeakerId");

                    b.HasIndex("SpeakerId");

                    b.ToTable("SessionSpeaker");
                });

            modelBuilder.Entity("BackEnd.Data.SessionTag", b =>
                {
                    b.Property<int>("SessionId");

                    b.Property<int>("TagId");

                    b.HasKey("SessionId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("SessionTag");
                });

            modelBuilder.Entity("BackEnd.Data.Speaker", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bio")
                        .HasMaxLength(4000);

                    b.Property<int?>("ConferenceID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("WebSite")
                        .HasMaxLength(1000);

                    b.HasKey("ID");

                    b.HasIndex("ConferenceID");

                    b.ToTable("Speakers");
                });

            modelBuilder.Entity("BackEnd.Data.Tag", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("ID");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("BackEnd.Data.Track", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ConferenceId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("ID");

                    b.HasIndex("ConferenceId");

                    b.ToTable("Tracks");
                });

            modelBuilder.Entity("BackEnd.Data.ConferenceAttendee", b =>
                {
                    b.HasOne("BackEnd.Data.Attendee", "Attendee")
                        .WithMany("ConferenceAttendees")
                        .HasForeignKey("AttendeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BackEnd.Data.Conference", "Conference")
                        .WithMany("ConferenceAttendees")
                        .HasForeignKey("ConferenceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BackEnd.Data.Image", b =>
                {
                    b.HasOne("BackEnd.Data.Attendee", "Attendee")
                        .WithMany("Images")
                        .HasForeignKey("AttendeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BackEnd.Data.Session", b =>
                {
                    b.HasOne("BackEnd.Data.Conference", "Conference")
                        .WithMany("Sessions")
                        .HasForeignKey("ConferenceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BackEnd.Data.Track", "Track")
                        .WithMany("Sessions")
                        .HasForeignKey("TrackId");
                });

            modelBuilder.Entity("BackEnd.Data.SessionAttendee", b =>
                {
                    b.HasOne("BackEnd.Data.Attendee", "Attendee")
                        .WithMany("SessionAttendees")
                        .HasForeignKey("AttendeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BackEnd.Data.Session", "Session")
                        .WithMany()
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BackEnd.Data.SessionSpeaker", b =>
                {
                    b.HasOne("BackEnd.Data.Session", "Session")
                        .WithMany("SessionSpeakers")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BackEnd.Data.Speaker", "Speaker")
                        .WithMany("SessionSpeakers")
                        .HasForeignKey("SpeakerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BackEnd.Data.SessionTag", b =>
                {
                    b.HasOne("BackEnd.Data.Session", "Session")
                        .WithMany("SessionTags")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BackEnd.Data.Tag", "Tag")
                        .WithMany("SessionTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BackEnd.Data.Speaker", b =>
                {
                    b.HasOne("BackEnd.Data.Conference")
                        .WithMany("Speakers")
                        .HasForeignKey("ConferenceID");
                });

            modelBuilder.Entity("BackEnd.Data.Track", b =>
                {
                    b.HasOne("BackEnd.Data.Conference", "Conference")
                        .WithMany("Tracks")
                        .HasForeignKey("ConferenceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
