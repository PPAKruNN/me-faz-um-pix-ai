﻿// <auto-generated />
using System;
using FazUmPix.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace fazumpix.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240310040641_create_BTree_index_on_UserCPF_PaymentProviderToken_and_PixKeyType")]
    partial class create_BTree_index_on_UserCPF_PaymentProviderToken_and_PixKeyType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FazUmPix.Models.PaymentProvider", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("Token")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Token")
                        .HasDatabaseName("IX_PaymentProvider_Token");

                    b.ToTable("PaymentProvider");
                });

            modelBuilder.Entity("FazUmPix.Models.PaymentProviderAccount", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("Agency")
                        .HasColumnType("integer");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<long>("PaymentProviderId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("PaymentProviderId");

                    b.HasIndex("UserId");

                    b.ToTable("PaymentProviderAccount");
                });

            modelBuilder.Entity("FazUmPix.Models.PixKey", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("PaymentProviderAccountId")
                        .HasColumnType("bigint");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PaymentProviderAccountId");

                    b.HasIndex("Type")
                        .HasDatabaseName("IX_PixKey_Type");

                    b.ToTable("PixKey");
                });

            modelBuilder.Entity("FazUmPix.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CPF")
                        .HasDatabaseName("IX_User_CPF");

                    b.ToTable("User");
                });

            modelBuilder.Entity("FazUmPix.Models.PaymentProviderAccount", b =>
                {
                    b.HasOne("FazUmPix.Models.PaymentProvider", "Bank")
                        .WithMany("Accounts")
                        .HasForeignKey("PaymentProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FazUmPix.Models.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FazUmPix.Models.PixKey", b =>
                {
                    b.HasOne("FazUmPix.Models.PaymentProviderAccount", "Account")
                        .WithMany("PixKeys")
                        .HasForeignKey("PaymentProviderAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("FazUmPix.Models.PaymentProvider", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("FazUmPix.Models.PaymentProviderAccount", b =>
                {
                    b.Navigation("PixKeys");
                });

            modelBuilder.Entity("FazUmPix.Models.User", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
