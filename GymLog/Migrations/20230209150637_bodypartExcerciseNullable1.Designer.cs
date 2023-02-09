﻿// <auto-generated />
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
    [Migration("20230209150637_bodypartExcerciseNullable1")]
    partial class bodypartExcerciseNullable1
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

                    b.Property<int?>("WeightType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Excercises");
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

            modelBuilder.Entity("GymLog.Models.BodyPart", b =>
                {
                    b.Navigation("BodyPartExcercises");
                });

            modelBuilder.Entity("GymLog.Models.Excercise", b =>
                {
                    b.Navigation("BodyPartExcercises");
                });
#pragma warning restore 612, 618
        }
    }
}
