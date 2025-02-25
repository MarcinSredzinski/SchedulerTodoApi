﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchedulerTodo.DB;

#nullable disable

namespace SchedulerTodo.Migrations
{
    [DbContext(typeof(SqlServerDbContext))]
    [Migration("20250224211349_ApiKeyAdded")]
    partial class ApiKeyAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Business.Library.Models.ApiKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Expiration")
                        .HasColumnType("datetime2");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApiKeys");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Key = "k8FZGeZg#I#6b1SwblyU^49TeZLtHLP!y!sB2boP*djNMFosfd"
                        });
                });

            modelBuilder.Entity("Business.Library.Models.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AppointmentType")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("DeletedOnUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Repeats")
                        .HasColumnType("int");

                    b.Property<DateTime>("RepeatsTo")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("SeriesIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime2");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted")
                        .HasFilter("IsDeleted = 0");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("Business.Library.Models.ItemToDo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("Deadline")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeletedOnUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsChecked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTimeOffset?>("RemindOn")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("ItemsToDo");
                });
#pragma warning restore 612, 618
        }
    }
}
