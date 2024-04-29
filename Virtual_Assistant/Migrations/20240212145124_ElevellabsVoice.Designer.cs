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
    [Migration("20240212145124_ElevellabsVoice")]
    partial class ElevellabsVoice
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

                    b.Property<string>("CharacterAiHistoryId")
                        .HasColumnType("TEXT");

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

            modelBuilder.Entity("Virtual_Assistant.Models.Hotkey", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("VirtualKeyCodesJson")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("VirtualKeyCodes");

                    b.HasKey("Id");

                    b.ToTable("hotkeys");
                });

            modelBuilder.Entity("Virtual_Assistant.Models.PersonaSingle", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CharacterDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StageName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("personas");
                });

            modelBuilder.Entity("Virtual_Assistant.Models.RoleplayCharacter", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CharacterAiId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CharacterAiTargetPersona")
                        .HasColumnType("TEXT");

                    b.Property<string>("CharacterName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ElevenlabsSelectedVoice")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCharacterAi")
                        .HasColumnType("INTEGER");

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

                    b.Property<string>("CharacterAiToken")
                        .HasColumnType("TEXT");

                    b.Property<string>("ElevenlabsApiKey")
                        .HasColumnType("TEXT");

                    b.Property<int>("GpuLayerCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LocalModel")
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxTokens")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SettingsTarget")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Temperature")
                        .HasColumnType("REAL");

                    b.Property<int>("TopK")
                        .HasColumnType("INTEGER");

                    b.Property<float>("TopP")
                        .HasColumnType("REAL");

                    b.Property<int>("WhisperModel")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

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

            modelBuilder.Entity("Virtual_Assistant.Models.ChatChannel", b =>
                {
                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
