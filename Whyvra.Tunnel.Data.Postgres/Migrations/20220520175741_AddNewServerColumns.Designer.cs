﻿// <auto-generated />
using System;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Whyvra.Tunnel.Data;

#nullable disable

namespace Whyvra.Tunnel.Data.Postgres.Migrations
{
    [DbContext(typeof(TunnelContext))]
    [Migration("20220520175741_AddNewServerColumns")]
    partial class AddNewServerColumns
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Whyvra.Tunnel.Domain.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<ValueTuple<IPAddress, int>>("AssignedIp")
                        .HasColumnType("cidr");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasMaxLength(44)
                        .HasColumnType("character varying(44)");

                    b.Property<int>("ServerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ServerId");

                    b.HasIndex("AssignedIp", "ServerId")
                        .IsUnique();

                    b.HasIndex("Name", "ServerId")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Whyvra.Tunnel.Domain.Entities.ClientNetworkAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("NetworkAddressId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("NetworkAddressId", "ClientId")
                        .IsUnique();

                    b.ToTable("ClientNetworkAddresses");
                });

            modelBuilder.Entity("Whyvra.Tunnel.Domain.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<JsonDocument>("Data")
                        .HasColumnType("jsonb");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<int>("RecordId")
                        .HasColumnType("integer");

                    b.Property<string>("SourceAddress")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)");

                    b.Property<string>("TableId")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Whyvra.Tunnel.Domain.Entities.NetworkAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<ValueTuple<IPAddress, int>>("Address")
                        .HasColumnType("cidr");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Address")
                        .IsUnique();

                    b.ToTable("NetworkAddresses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = new System.ValueTuple<System.Net.IPAddress, int>(System.Net.IPAddress.Parse("0.0.0.0"), 0),
                            CreatedAt = new DateTime(2020, 10, 25, 21, 47, 35, 59, DateTimeKind.Utc).AddTicks(7650),
                            UpdatedAt = new DateTime(2020, 10, 25, 21, 47, 35, 59, DateTimeKind.Utc).AddTicks(7650)
                        },
                        new
                        {
                            Id = 2,
                            Address = new System.ValueTuple<System.Net.IPAddress, int>(System.Net.IPAddress.Parse("::"), 0),
                            CreatedAt = new DateTime(2020, 10, 25, 21, 47, 35, 59, DateTimeKind.Utc).AddTicks(7650),
                            UpdatedAt = new DateTime(2020, 10, 25, 21, 47, 35, 59, DateTimeKind.Utc).AddTicks(7650)
                        });
                });

            modelBuilder.Entity("Whyvra.Tunnel.Domain.Entities.Server", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool?>("AddFirewallRules")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnOrder(11);

                    b.Property<ValueTuple<IPAddress, int>>("AssignedRange")
                        .HasColumnType("cidr")
                        .HasColumnOrder(4);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnOrder(15);

                    b.Property<string>("CustomConfiguration")
                        .HasColumnType("text")
                        .HasColumnOrder(13);

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnOrder(3);

                    b.Property<IPAddress>("Dns")
                        .IsRequired()
                        .HasColumnType("inet")
                        .HasColumnOrder(5);

                    b.Property<string>("Endpoint")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnOrder(6);

                    b.Property<int>("ListenPort")
                        .HasColumnType("integer")
                        .HasColumnOrder(7);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnOrder(2);

                    b.Property<string>("NetworkInterface")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)")
                        .HasDefaultValue("eth0")
                        .HasColumnOrder(12);

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasMaxLength(44)
                        .HasColumnType("character varying(44)")
                        .HasColumnOrder(8);

                    b.Property<bool>("RenderToDisk")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnOrder(10);

                    b.Property<string>("StatusApi")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnOrder(9);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnOrder(16);

                    b.Property<string>("WireGuardInterface")
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)")
                        .HasColumnOrder(14);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("WireGuardInterface")
                        .IsUnique();

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("Whyvra.Tunnel.Domain.Entities.ServerNetworkAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("NetworkAddressId")
                        .HasColumnType("integer");

                    b.Property<int>("ServerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ServerId");

                    b.HasIndex("NetworkAddressId", "ServerId")
                        .IsUnique();

                    b.ToTable("ServerNetworkAddresses");
                });

            modelBuilder.Entity("Whyvra.Tunnel.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<Guid>("Uid")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2020, 10, 25, 21, 47, 35, 65, DateTimeKind.Utc).AddTicks(993),
                            Email = "system@example.com",
                            FirstName = "System",
                            LastName = "User",
                            Uid = new Guid("e3adf55b-7430-42c1-ae62-758d7b644fdb"),
                            UpdatedAt = new DateTime(2020, 10, 25, 21, 47, 35, 65, DateTimeKind.Utc).AddTicks(993),
                            Username = "system_user"
                        });
                });

            modelBuilder.Entity("Whyvra.Tunnel.Domain.Entities.Client", b =>
                {
                    b.HasOne("Whyvra.Tunnel.Domain.Entities.Server", "Server")
                        .WithMany("Clients")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Server");
                });

            modelBuilder.Entity("Whyvra.Tunnel.Domain.Entities.ClientNetworkAddress", b =>
                {
                    b.HasOne("Whyvra.Tunnel.Domain.Entities.Client", "Client")
                        .WithMany("AllowedIps")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Whyvra.Tunnel.Domain.Entities.NetworkAddress", "NetworkAddress")
                        .WithMany()
                        .HasForeignKey("NetworkAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("NetworkAddress");
                });

            modelBuilder.Entity("Whyvra.Tunnel.Domain.Entities.Event", b =>
                {
                    b.HasOne("Whyvra.Tunnel.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Whyvra.Tunnel.Domain.Entities.ServerNetworkAddress", b =>
                {
                    b.HasOne("Whyvra.Tunnel.Domain.Entities.NetworkAddress", "NetworkAddress")
                        .WithMany()
                        .HasForeignKey("NetworkAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Whyvra.Tunnel.Domain.Entities.Server", "Server")
                        .WithMany("DefaultAllowedRange")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NetworkAddress");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("Whyvra.Tunnel.Domain.Entities.Client", b =>
                {
                    b.Navigation("AllowedIps");
                });

            modelBuilder.Entity("Whyvra.Tunnel.Domain.Entities.Server", b =>
                {
                    b.Navigation("Clients");

                    b.Navigation("DefaultAllowedRange");
                });
#pragma warning restore 612, 618
        }
    }
}
