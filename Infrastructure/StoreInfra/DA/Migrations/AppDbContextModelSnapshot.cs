﻿// <auto-generated />
using System;
using DA.AppDbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DA.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DM.DomainModels.CashManagement", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("CashManagements");
                });

            modelBuilder.Entity("DM.DomainModels.CustomerCart", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CustomerId")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsConvertedToSale")
                        .HasColumnType("boolean");

                    b.Property<string>("TillId")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("TillId");

                    b.ToTable("CustomerCart");
                });

            modelBuilder.Entity("DM.DomainModels.CustomerCartItems", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CartId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<string>("ItemId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.ToTable("CustomerCartItems");
                });

            modelBuilder.Entity("DM.DomainModels.CustomerManagement", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Billing")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Cnic")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("CustomerManagements");
                });

            modelBuilder.Entity("DM.DomainModels.Order", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CartId")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("boolean");

                    b.Property<int>("PaidAmount")
                        .HasColumnType("integer");

                    b.Property<int>("TotalAmount")
                        .HasColumnType("integer");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("DM.DomainModels.OrderDetails", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<string>("ItemName")
                        .HasColumnType("text");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("DM.DomainModels.OrderSplitPayments", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<string>("OrderId")
                        .HasColumnType("text");

                    b.Property<string>("PaymentMethodId")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("PaymentMethodId");

                    b.ToTable("OrderSplitPayments");
                });

            modelBuilder.Entity("DM.DomainModels.PaymentMethods", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("PaymentMethods");

                    b.HasData(
                        new
                        {
                            Id = "a7dabf7f-9718-4587-80cf-b678ec8c9853",
                            CreatedBy = "Default",
                            CreatedDate = new DateTime(2024, 9, 13, 14, 12, 24, 192, DateTimeKind.Utc).AddTicks(7597),
                            IsActive = true,
                            IsArchived = false,
                            Name = "Cash",
                            UpdatedBy = "",
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = "272e3baa-ae7d-4355-80f0-3d29e1985c71",
                            CreatedBy = "Default",
                            CreatedDate = new DateTime(2024, 9, 13, 14, 12, 24, 192, DateTimeKind.Utc).AddTicks(7658),
                            IsActive = true,
                            IsArchived = false,
                            Name = "Card",
                            UpdatedBy = "",
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = "d41523b9-e04e-4a41-8658-7c40853d71c5",
                            CreatedBy = "Default",
                            CreatedDate = new DateTime(2024, 9, 13, 14, 12, 24, 192, DateTimeKind.Utc).AddTicks(7664),
                            IsActive = true,
                            IsArchived = false,
                            Name = "Gift Card",
                            UpdatedBy = "",
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("DM.DomainModels.PaymentTransactions", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Amount")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<string>("SplitPaymentId")
                        .HasColumnType("text");

                    b.Property<string>("TaxId")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("SplitPaymentId");

                    b.ToTable("PaymentTransactions");
                });

            modelBuilder.Entity("DM.DomainModels.Till", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Till");
                });

            modelBuilder.Entity("DM.DomainModels.CustomerCart", b =>
                {
                    b.HasOne("DM.DomainModels.CustomerManagement", "CustomerManagement")
                        .WithMany("CustomerCarts")
                        .HasForeignKey("CustomerId");

                    b.HasOne("DM.DomainModels.Till", "Till")
                        .WithMany("Carts")
                        .HasForeignKey("TillId");

                    b.Navigation("CustomerManagement");

                    b.Navigation("Till");
                });

            modelBuilder.Entity("DM.DomainModels.CustomerCartItems", b =>
                {
                    b.HasOne("DM.DomainModels.CustomerCart", "CustomerCart")
                        .WithMany("CustomerCartItems")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomerCart");
                });

            modelBuilder.Entity("DM.DomainModels.Order", b =>
                {
                    b.HasOne("DM.DomainModels.CustomerCart", "CustomerCart")
                        .WithMany("Order")
                        .HasForeignKey("CartId");

                    b.Navigation("CustomerCart");
                });

            modelBuilder.Entity("DM.DomainModels.OrderSplitPayments", b =>
                {
                    b.HasOne("DM.DomainModels.Order", "Order")
                        .WithMany("OrderSplitPayments")
                        .HasForeignKey("OrderId");

                    b.HasOne("DM.DomainModels.PaymentMethods", "PaymentMethods")
                        .WithMany("OrderSplitPayments")
                        .HasForeignKey("PaymentMethodId");

                    b.Navigation("Order");

                    b.Navigation("PaymentMethods");
                });

            modelBuilder.Entity("DM.DomainModels.PaymentTransactions", b =>
                {
                    b.HasOne("DM.DomainModels.OrderSplitPayments", "OrderSplitPayments")
                        .WithMany("PaymentTransactions")
                        .HasForeignKey("SplitPaymentId");

                    b.Navigation("OrderSplitPayments");
                });

            modelBuilder.Entity("DM.DomainModels.CustomerCart", b =>
                {
                    b.Navigation("CustomerCartItems");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("DM.DomainModels.CustomerManagement", b =>
                {
                    b.Navigation("CustomerCarts");
                });

            modelBuilder.Entity("DM.DomainModels.Order", b =>
                {
                    b.Navigation("OrderSplitPayments");
                });

            modelBuilder.Entity("DM.DomainModels.OrderSplitPayments", b =>
                {
                    b.Navigation("PaymentTransactions");
                });

            modelBuilder.Entity("DM.DomainModels.PaymentMethods", b =>
                {
                    b.Navigation("OrderSplitPayments");
                });

            modelBuilder.Entity("DM.DomainModels.Till", b =>
                {
                    b.Navigation("Carts");
                });
#pragma warning restore 612, 618
        }
    }
}
