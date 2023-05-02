﻿// <auto-generated />
using System;
using GymLog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GymLog.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230421195925_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GymLog.Models.BodyPart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BodyParts");
                });

            modelBuilder.Entity("GymLog.Models.BodyPartExcercise", b =>
                {
                    b.Property<int>("BodyPartId")
                        .HasColumnType("int");

                    b.Property<int>("ExcerciseId")
                        .HasColumnType("int");

                    b.HasKey("BodyPartId", "ExcerciseId");

                    b.HasIndex("ExcerciseId");

                    b.ToTable("BodyPartExcercises");
                });

            modelBuilder.Entity("GymLog.Models.Excercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Excercises");
                });

            modelBuilder.Entity("GymLog.Models.Set", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Reps")
                        .HasColumnType("int");

                    b.Property<int>("WorkoutSegmentId")
                        .HasColumnType("int");

                    b.Property<float?>("Weight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutSegmentId");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("GymLog.Models.Template", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("GymLog.Models.WorkoutSegment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExcerciseId")
                        .HasColumnType("int");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<int?>("TemplateId")
                        .HasColumnType("int");

                    b.Property<int?>("WeightType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExcerciseId");

                    b.HasIndex("TemplateId");

                    b.ToTable("WorkoutSegments");
                });

            modelBuilder.Entity("GymLog.Models.Workout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("TemplateId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TemplateId");

                    b.ToTable("Workouts");
                });

            modelBuilder.Entity("GymLog.Models.BodyPartExcercise", b =>
                {
                    b.HasOne("GymLog.Models.BodyPart", "BodyPart")
                        .WithMany("BodyPartExcercises")
                        .HasForeignKey("BodyPartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymLog.Models.Excercise", "Excercise")
                        .WithMany("BodyPartExcercises")
                        .HasForeignKey("ExcerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BodyPart");

                    b.Navigation("Excercise");
                });

            modelBuilder.Entity("GymLog.Models.Set", b =>
                {
                    b.HasOne("GymLog.Models.WorkoutSegment", "WorkoutSegment")
                        .WithMany("Sets")
                        .HasForeignKey("WorkoutSegmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkoutSegment");
                });

            modelBuilder.Entity("GymLog.Models.WorkoutSegment", b =>
                {
                    b.HasOne("GymLog.Models.Excercise", "Excercise")
                        .WithMany("WorkoutSegment")
                        .HasForeignKey("ExcerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymLog.Models.Template", "Template")
                        .WithMany("WorkoutSegments")
                        .HasForeignKey("TemplateId");

                    b.Navigation("Excercise");

                    b.Navigation("Template");
                });

            modelBuilder.Entity("GymLog.Models.Workout", b =>
                {
                    b.HasOne("GymLog.Models.Template", "Template")
                        .WithMany()
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Template");
                });

            modelBuilder.Entity("GymLog.Models.BodyPart", b =>
                {
                    b.Navigation("BodyPartExcercises");
                });

            modelBuilder.Entity("GymLog.Models.Excercise", b =>
                {
                    b.Navigation("BodyPartExcercises");

                    b.Navigation("WorkoutSegment");
                });

            modelBuilder.Entity("GymLog.Models.Template", b =>
                {
                    b.Navigation("WorkoutSegments");
                });

            modelBuilder.Entity("GymLog.Models.WorkoutSegment", b =>
                {
                    b.Navigation("Sets");
                });
#pragma warning restore 612, 618
        }
    }
}
