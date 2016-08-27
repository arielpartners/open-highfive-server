﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HighFive.Server.Api.Models;

namespace HighFive.Server.Migrations
{
    [DbContext(typeof(HighFiveContext))]
    partial class HighFiveContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HighFive.Server.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Description");

                    b.Property<int?>("RecognitionId");

                    b.Property<Guid?>("SenderId");

                    b.HasKey("Id");

                    b.HasIndex("RecognitionId");

                    b.HasIndex("SenderId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("HighFive.Server.Models.CorporateValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<Guid?>("OrganizationId");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("CorporateValues");
                });

            modelBuilder.Entity("HighFive.Server.Models.HighFiveUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<Guid?>("OrganizationId");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HighFive.Server.Models.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("HighFive.Server.Models.Recognition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Description");

                    b.Property<Guid?>("OrganizationId");

                    b.Property<Guid?>("ReceiverId");

                    b.Property<Guid?>("SenderId");

                    b.Property<Guid?>("ValueId");

                    b.Property<bool>("isPrivate");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.HasIndex("ValueId");

                    b.ToTable("Recognitions");
                });

            modelBuilder.Entity("HighFive.Server.Models.Comment", b =>
                {
                    b.HasOne("HighFive.Server.Models.Recognition")
                        .WithMany("Comments")
                        .HasForeignKey("RecognitionId");

                    b.HasOne("HighFive.Server.Models.HighFiveUser", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");
                });

            modelBuilder.Entity("HighFive.Server.Models.CorporateValue", b =>
                {
                    b.HasOne("HighFive.Server.Models.Organization")
                        .WithMany("Values")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("HighFive.Server.Models.HighFiveUser", b =>
                {
                    b.HasOne("HighFive.Server.Models.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("HighFive.Server.Models.Recognition", b =>
                {
                    b.HasOne("HighFive.Server.Models.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId");

                    b.HasOne("HighFive.Server.Models.HighFiveUser", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId");

                    b.HasOne("HighFive.Server.Models.HighFiveUser", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");

                    b.HasOne("HighFive.Server.Models.CorporateValue", "Value")
                        .WithMany()
                        .HasForeignKey("ValueId");
                });
        }
    }
}
