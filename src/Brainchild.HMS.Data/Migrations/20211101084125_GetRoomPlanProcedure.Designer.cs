﻿// <auto-generated />
using System;
using Brainchild.HMS.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Brainchild.HMS.Data.Migrations
{
    [DbContext(typeof(BrainchildHMSDbContext))]
    [Migration("20211101084125_GetRoomPlanProcedure")]
    partial class GetRoomPlanProcedure
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Billing", b =>
                {
                    b.Property<int>("BillingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BillingDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("BookingId")
                        .HasColumnType("int");

                    b.Property<int?>("ChargeId")
                        .HasColumnType("int");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("BillingId");

                    b.HasIndex("BookingId");

                    b.HasIndex("ChargeId");

                    b.HasIndex("RoomId");

                    b.ToTable("Billings");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Booking", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CancelledDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CheckInDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CheckOutDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("GuestId")
                        .HasColumnType("int");

                    b.Property<int?>("HotelId")
                        .HasColumnType("int");

                    b.Property<int>("IsCancelled")
                        .HasColumnType("int");

                    b.Property<int>("NoOfAdults")
                        .HasColumnType("int");

                    b.Property<int>("NoOfChildren")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("BookingId");

                    b.HasIndex("GuestId");

                    b.HasIndex("HotelId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Charge", b =>
                {
                    b.Property<int>("ChargeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BookingId")
                        .HasColumnType("int");

                    b.Property<float>("ChargeAmount")
                        .HasColumnType("real");

                    b.Property<int?>("ChargeTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("ChargeId");

                    b.HasIndex("BookingId");

                    b.HasIndex("ChargeTypeId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("RoomId");

                    b.ToTable("Charges");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.ChargeType", b =>
                {
                    b.Property<int>("ChargeTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChargeTypeDescription")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.Property<int?>("HotelId")
                        .HasColumnType("int");

                    b.HasKey("ChargeTypeId");

                    b.HasIndex("HotelId");

                    b.ToTable("ChargeTypes");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Currency", b =>
                {
                    b.Property<int>("CurrencyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("CurrencyCountry")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("CurrencyNumber")
                        .HasColumnType("int");

                    b.Property<string>("CurrencySymbol")
                        .HasColumnType("varchar(100)");

                    b.HasKey("CurrencyId");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Guest", b =>
                {
                    b.Property<int>("GuestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GuestAddress")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("GuestCountry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GuestEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GuestName")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("GuestPhoneNo")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("GuestId");

                    b.ToTable("Guests");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Hotel", b =>
                {
                    b.Property<int>("HotelID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GSTNo")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("HotelEmail")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("HotelPhone")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("OwnerName")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("Place")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("RegistrationNo")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("TenantID")
                        .HasColumnType("int");

                    b.HasKey("HotelID");

                    b.ToTable("Hotels");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Note", b =>
                {
                    b.Property<int>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BookingId")
                        .HasColumnType("int");

                    b.Property<string>("NoteDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NoteId");

                    b.HasIndex("BookingId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BillingId")
                        .HasColumnType("int");

                    b.Property<float>("PaymentAmount")
                        .HasColumnType("real");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PaymentTypeId")
                        .HasColumnType("int");

                    b.HasKey("PaymentId");

                    b.HasIndex("BillingId");

                    b.HasIndex("PaymentTypeId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.PaymentType", b =>
                {
                    b.Property<int>("PaymentTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("HotelId")
                        .HasColumnType("int");

                    b.Property<string>("PaymentTypeDescription")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.HasKey("PaymentTypeID");

                    b.HasIndex("HotelId");

                    b.ToTable("PaymentTypes");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("HotelId")
                        .HasColumnType("int");

                    b.Property<string>("RoomNo")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("RoomStatus")
                        .HasColumnType("int");

                    b.Property<int?>("RoomTypeId")
                        .HasColumnType("int");

                    b.HasKey("RoomId");

                    b.HasIndex("HotelId");

                    b.HasIndex("RoomNo");

                    b.HasIndex("RoomTypeId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.RoomBooking", b =>
                {
                    b.Property<int>("RoomBookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BookingId")
                        .HasColumnType("int");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("RoomBookingId");

                    b.HasIndex("BookingId");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomBookings");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.RoomType", b =>
                {
                    b.Property<int>("RoomTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("HotelId")
                        .HasColumnType("int");

                    b.Property<float>("RoomRate")
                        .HasColumnType("real");

                    b.Property<string>("RoomTypeDesctiption")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.HasKey("RoomTypeId");

                    b.HasIndex("HotelId");

                    b.ToTable("RoomTypes");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Tax", b =>
                {
                    b.Property<int>("TaxId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("HotelId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TaxDecription")
                        .HasColumnType("varchar(1000)");

                    b.Property<int>("TaxPercentage")
                        .HasColumnType("int");

                    b.HasKey("TaxId");

                    b.HasIndex("HotelId");

                    b.ToTable("Taxes");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Billing", b =>
                {
                    b.HasOne("Brainchild.HMS.Core.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingId");

                    b.HasOne("Brainchild.HMS.Core.Models.Charge", "Charge")
                        .WithMany()
                        .HasForeignKey("ChargeId");

                    b.HasOne("Brainchild.HMS.Core.Models.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId");

                    b.Navigation("Booking");

                    b.Navigation("Charge");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Booking", b =>
                {
                    b.HasOne("Brainchild.HMS.Core.Models.Guest", "Guest")
                        .WithMany()
                        .HasForeignKey("GuestId");

                    b.HasOne("Brainchild.HMS.Core.Models.Hotel", "Hotel")
                        .WithMany()
                        .HasForeignKey("HotelId");

                    b.Navigation("Guest");

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Charge", b =>
                {
                    b.HasOne("Brainchild.HMS.Core.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingId");

                    b.HasOne("Brainchild.HMS.Core.Models.ChargeType", "ChargeType")
                        .WithMany()
                        .HasForeignKey("ChargeTypeId");

                    b.HasOne("Brainchild.HMS.Core.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId");

                    b.HasOne("Brainchild.HMS.Core.Models.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId");

                    b.Navigation("Booking");

                    b.Navigation("ChargeType");

                    b.Navigation("Currency");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.ChargeType", b =>
                {
                    b.HasOne("Brainchild.HMS.Core.Models.Hotel", "Hotel")
                        .WithMany()
                        .HasForeignKey("HotelId");

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Note", b =>
                {
                    b.HasOne("Brainchild.HMS.Core.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingId");

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Payment", b =>
                {
                    b.HasOne("Brainchild.HMS.Core.Models.Billing", "Billing")
                        .WithMany("Payments")
                        .HasForeignKey("BillingId");

                    b.HasOne("Brainchild.HMS.Core.Models.PaymentType", "PaymentType")
                        .WithMany()
                        .HasForeignKey("PaymentTypeId");

                    b.Navigation("Billing");

                    b.Navigation("PaymentType");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.PaymentType", b =>
                {
                    b.HasOne("Brainchild.HMS.Core.Models.Hotel", "Hotel")
                        .WithMany()
                        .HasForeignKey("HotelId");

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Room", b =>
                {
                    b.HasOne("Brainchild.HMS.Core.Models.Hotel", "Hotel")
                        .WithMany()
                        .HasForeignKey("HotelId");

                    b.HasOne("Brainchild.HMS.Core.Models.RoomType", "RoomType")
                        .WithMany()
                        .HasForeignKey("RoomTypeId");

                    b.Navigation("Hotel");

                    b.Navigation("RoomType");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.RoomBooking", b =>
                {
                    b.HasOne("Brainchild.HMS.Core.Models.Booking", "Booking")
                        .WithMany("RoomBookings")
                        .HasForeignKey("BookingId");

                    b.HasOne("Brainchild.HMS.Core.Models.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId");

                    b.Navigation("Booking");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.RoomType", b =>
                {
                    b.HasOne("Brainchild.HMS.Core.Models.Hotel", "Hotel")
                        .WithMany()
                        .HasForeignKey("HotelId");

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Tax", b =>
                {
                    b.HasOne("Brainchild.HMS.Core.Models.Hotel", "Hotel")
                        .WithMany()
                        .HasForeignKey("HotelId");

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Billing", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("Brainchild.HMS.Core.Models.Booking", b =>
                {
                    b.Navigation("RoomBookings");
                });
#pragma warning restore 612, 618
        }
    }
}
