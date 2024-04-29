﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Virtual_Assistant.Data;

#nullable disable

namespace Virtual_Assistant.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240203145811_RemoveIncrementId")]
    partial class RemoveIncrementId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("ChatChannelRoleplayCharacter", b =>
                {
                    b.Property<long>("CharacterChannelsId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("CharactersId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CharacterChannelsId", "CharactersId");

                    b.HasIndex("CharactersId");

                    b.ToTable("ChatChannelRoleplayCharacter");
                });

            modelBuilder.Entity("Virtual_Assistant.Models.ChatChannel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("channels");
                });

            modelBuilder.Entity("Virtual_Assistant.Models.ChatMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("ChatChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("Sender")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("SentByUser")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ChatChannelId");

                    b.ToTable("messages");
                });

            modelBuilder.Entity("Virtual_Assistant.Models.LocalLlamaModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FilePath")
                        .HasColumnType("TEXT");

                    b.Property<string>("HuggingFaceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModelHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Source")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("models");
                });

            modelBuilder.Entity("Virtual_Assistant.Models.RoleplayCharacter", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CharacterName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProfilePictureHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SampleMessages")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("characters");
                });

            modelBuilder.Entity("Virtual_Assistant.Models.Settings", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxTokens")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SettingsTarget")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("TargetModelId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Temperature")
                        .HasColumnType("REAL");

                    b.Property<int>("TopK")
                        .HasColumnType("INTEGER");

                    b.Property<float>("TopP")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("TargetModelId");

                    b.ToTable("settings");
                });

            modelBuilder.Entity("ChatChannelRoleplayCharacter", b =>
                {
                    b.HasOne("Virtual_Assistant.Models.ChatChannel", null)
                        .WithMany()
                        .HasForeignKey("CharacterChannelsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Virtual_Assistant.Models.RoleplayCharacter", null)
                        .WithMany()
                        .HasForeignKey("CharactersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Virtual_Assistant.Models.ChatMessage", b =>
                {
                    b.HasOne("Virtual_Assistant.Models.ChatChannel", "ChatChannel")
                        .WithMany("Messages")
                        .HasForeignKey("ChatChannelId");

                    b.Navigation("ChatChannel");
                });

            modelBuilder.Entity("Virtual_Assistant.Models.Settings", b =>
                {
                    b.HasOne("Virtual_Assistant.Models.LocalLlamaModel", "TargetModel")
                        .WithMany()
                        .HasForeignKey("TargetModelId");

                    b.Navigation("TargetModel");
                });

            modelBuilder.Entity("Virtual_Assistant.Models.ChatChannel", b =>
                {
                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
