﻿// <auto-generated />
using System;
using ExpensesManager.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExpensesManager.DB.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220929090719_updateTotalExpensePerCategory")]
    partial class updateTotalExpensePerCategory
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("ExpensesManager.DB.Models.Categories", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("MappedCategoriesJson")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ExpensesManager.DB.Models.ExpenseRecord", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Card_Details")
                        .HasColumnType("TEXT");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Currency")
                        .HasColumnType("TEXT");

                    b.Property<double>("Debit_Amount")
                        .HasColumnType("REAL");

                    b.Property<string>("Exchange_Description")
                        .HasColumnType("TEXT");

                    b.Property<double>("Exchange_Rate")
                        .HasColumnType("REAL");

                    b.Property<string>("Expense_Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Linked_Month")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double?>("Price_Amount")
                        .HasColumnType("REAL");

                    b.Property<string>("Record_Create_Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Transaction_Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("User_ID")
                        .HasColumnType("INTEGER");

                    b.HasKey("TransactionID");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("ExpensesManager.DB.Models.SwRecords", b =>
                {
                    b.Property<int>("Internal_TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Creation_Method")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Expense_Creation_Date")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Expense_Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Linked_Month")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Owed_Share")
                        .HasColumnType("REAL");

                    b.Property<double>("Paid_Share")
                        .HasColumnType("REAL");

                    b.Property<string>("Record_Creation_Date")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SW_TransactionID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SW_User_ID")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Total_Cost")
                        .HasColumnType("REAL");

                    b.HasKey("Internal_TransactionID");

                    b.ToTable("SpliteWise");
                });

            modelBuilder.Entity("ExpensesManager.DB.Models.TotalExpensePerCategory", b =>
                {
                    b.Property<int>("ItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Month")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SW_UserID")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Total_Amount")
                        .HasColumnType("REAL");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("ItemID");

                    b.ToTable("TotalExpensesPerCategory");
                });

            modelBuilder.Entity("ExpensesManager.DB.Models.Users", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SplitwisePassword")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
