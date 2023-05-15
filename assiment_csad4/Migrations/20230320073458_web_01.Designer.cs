﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using assiment_csad4.Configruration;

#nullable disable

namespace assiment_csad4.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20230320073458_web_01")]
    partial class web_01
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("assiment_csad4.Models.Bill", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Bill", (string)null);
                });

            modelBuilder.Entity("assiment_csad4.Models.BillDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<Guid>("IdHD")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdSP")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdHD");

                    b.HasIndex("IdSP");

                    b.ToTable("BillDetails");
                });

            modelBuilder.Entity("assiment_csad4.Models.Cart", b =>
                {
                    b.Property<Guid>("IdUser")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("IdUser");

                    b.ToTable("Cart", (string)null);
                });

            modelBuilder.Entity("assiment_csad4.Models.CartDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<Guid>("IdProduct")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdUser")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantilty")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdProduct");

                    b.HasIndex("IdUser");

                    b.ToTable("CartDetail", (string)null);
                });

            modelBuilder.Entity("assiment_csad4.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<int>("AvailableQuantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("0");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("Supplier")
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("assiment_csad4.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("((0))");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("assiment_csad4.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<Guid>("IdRole")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Pass")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("((0))");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.HasIndex("IdRole");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("assiment_csad4.Models.Bill", b =>
                {
                    b.HasOne("assiment_csad4.Models.User", "UserNavigation")
                        .WithMany("Bills")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserNavigation");
                });

            modelBuilder.Entity("assiment_csad4.Models.BillDetail", b =>
                {
                    b.HasOne("assiment_csad4.Models.Bill", "Bill")
                        .WithMany("BillDetails")
                        .HasForeignKey("IdHD")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_HD");

                    b.HasOne("assiment_csad4.Models.Product", "Product")
                        .WithMany("BillDetails")
                        .HasForeignKey("IdSP")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_SP");

                    b.Navigation("Bill");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("assiment_csad4.Models.Cart", b =>
                {
                    b.HasOne("assiment_csad4.Models.User", "User")
                        .WithOne("Cart")
                        .HasForeignKey("assiment_csad4.Models.Cart", "IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("assiment_csad4.Models.CartDetail", b =>
                {
                    b.HasOne("assiment_csad4.Models.Product", "ProductNavigation")
                        .WithMany("CartDetails")
                        .HasForeignKey("IdProduct")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("assiment_csad4.Models.Cart", "CartNavigation")
                        .WithMany("CartDetails")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CartNavigation");

                    b.Navigation("ProductNavigation");
                });

            modelBuilder.Entity("assiment_csad4.Models.User", b =>
                {
                    b.HasOne("assiment_csad4.Models.Role", "RoleNavigation")
                        .WithMany("Users")
                        .HasForeignKey("IdRole")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RoleNavigation");
                });

            modelBuilder.Entity("assiment_csad4.Models.Bill", b =>
                {
                    b.Navigation("BillDetails");
                });

            modelBuilder.Entity("assiment_csad4.Models.Cart", b =>
                {
                    b.Navigation("CartDetails");
                });

            modelBuilder.Entity("assiment_csad4.Models.Product", b =>
                {
                    b.Navigation("BillDetails");

                    b.Navigation("CartDetails");
                });

            modelBuilder.Entity("assiment_csad4.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("assiment_csad4.Models.User", b =>
                {
                    b.Navigation("Bills");

                    b.Navigation("Cart");
                });
#pragma warning restore 612, 618
        }
    }
}
