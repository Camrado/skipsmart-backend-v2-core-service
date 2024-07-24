﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SkipSmart.Infrastructure;

#nullable disable

namespace SkipSmart.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240724130802_Create_Database")]
    partial class Create_Database
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SkipSmart.Domain.Attendances.Attendance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateOnly>("AttendanceDate")
                        .HasColumnType("date")
                        .HasColumnName("attendance_date");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid")
                        .HasColumnName("course_id");

                    b.Property<bool>("HasAttended")
                        .HasColumnType("boolean")
                        .HasColumnName("has_attended");

                    b.Property<int>("Period")
                        .HasColumnType("integer")
                        .HasColumnName("period");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_attendances");

                    b.HasIndex("CourseId")
                        .HasDatabaseName("ix_attendances_course_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_attendances_user_id");

                    b.ToTable("attendances", (string)null);
                });

            modelBuilder.Entity("SkipSmart.Domain.CourseHours.CourseHour", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid")
                        .HasColumnName("course_id");

                    b.Property<decimal>("Hours")
                        .HasColumnType("numeric")
                        .HasColumnName("hours");

                    b.HasKey("Id")
                        .HasName("pk_course_hours");

                    b.HasIndex("CourseId")
                        .HasDatabaseName("ix_course_hours_course_id");

                    b.ToTable("course_hours", (string)null);
                });

            modelBuilder.Entity("SkipSmart.Domain.Courses.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("course_name");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("group_id");

                    b.Property<int>("Semester")
                        .HasColumnType("integer")
                        .HasColumnName("semester");

                    b.HasKey("Id")
                        .HasName("pk_courses");

                    b.HasIndex("GroupId")
                        .HasDatabaseName("ix_courses_group_id");

                    b.ToTable("courses", (string)null);
                });

            modelBuilder.Entity("SkipSmart.Domain.Groups.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("EdupageClassId")
                        .HasColumnType("integer")
                        .HasColumnName("edupage_class_id");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("group_name");

                    b.HasKey("Id")
                        .HasName("pk_groups");

                    b.ToTable("groups", (string)null);
                });

            modelBuilder.Entity("SkipSmart.Domain.MarkedDates.MarkedDate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateOnly>("RecordedDate")
                        .HasColumnType("date")
                        .HasColumnName("recorded_date");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_marked_dates");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_marked_dates_user_id");

                    b.ToTable("marked_dates", (string)null);
                });

            modelBuilder.Entity("SkipSmart.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("email");

                    b.Property<int?>("EmailVerificationCode")
                        .HasMaxLength(6)
                        .HasColumnType("integer")
                        .HasColumnName("email_verification_code");

                    b.Property<DateTime>("EmailVerificationSentAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("email_verification_sent_at");

                    b.Property<int>("FacultySubgroup")
                        .HasColumnType("integer")
                        .HasColumnName("faculty_subgroup");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("first_name");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("group_id");

                    b.Property<bool>("IsEmailVerified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_email_verified");

                    b.Property<int>("LanguageSubgroup")
                        .HasColumnType("integer")
                        .HasColumnName("language_subgroup");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("last_name");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_users_email");

                    b.HasIndex("GroupId")
                        .HasDatabaseName("ix_users_group_id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("SkipSmart.Domain.Attendances.Attendance", b =>
                {
                    b.HasOne("SkipSmart.Domain.Courses.Course", null)
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_attendances_course_course_id");

                    b.HasOne("SkipSmart.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_attendances_user_user_id");
                });

            modelBuilder.Entity("SkipSmart.Domain.CourseHours.CourseHour", b =>
                {
                    b.HasOne("SkipSmart.Domain.Courses.Course", null)
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_course_hours_courses_course_id");
                });

            modelBuilder.Entity("SkipSmart.Domain.Courses.Course", b =>
                {
                    b.HasOne("SkipSmart.Domain.Groups.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_courses_group_group_id");
                });

            modelBuilder.Entity("SkipSmart.Domain.MarkedDates.MarkedDate", b =>
                {
                    b.HasOne("SkipSmart.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_marked_dates_user_user_id");
                });

            modelBuilder.Entity("SkipSmart.Domain.Users.User", b =>
                {
                    b.HasOne("SkipSmart.Domain.Groups.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_users_groups_group_id");

                    b.OwnsOne("SkipSmart.Domain.Users.Password", "Password", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("HashedPassword")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("password_hashed_password");

                            b1.Property<string>("PasswordSalt")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("password_password_salt");

                            b1.HasKey("UserId");

                            b1.ToTable("users");

                            b1.WithOwner()
                                .HasForeignKey("UserId")
                                .HasConstraintName("fk_users_users_id");
                        });

                    b.Navigation("Password")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
