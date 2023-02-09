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
    [Migration("20230207171627_init1")]
    partial class init1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BodyPartExcercise", b =>
                {
                    b.Property<int>("BodyPartsId")
                        .HasColumnType("int");

                    b.Property<int>("ExcercisesId")
                        .HasColumnType("int");

                    b.HasKey("BodyPartsId", "ExcercisesId");

                    b.HasIndex("ExcercisesId");

                    b.ToTable("BodyPartExcercise");
                });

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

            modelBuilder.Entity("BodyPartExcercise", b =>
                {
                    b.HasOne("GymLog.Models.BodyPart", null)
                        .WithMany()
                        .HasForeignKey("BodyPartsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymLog.Models.Excercise", null)
                        .WithMany()
                        .HasForeignKey("ExcercisesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
