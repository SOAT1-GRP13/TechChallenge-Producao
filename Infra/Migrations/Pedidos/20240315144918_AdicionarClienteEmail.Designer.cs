﻿// <auto-generated />
using System;
using Infra.Pedidos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Migrations
{
    [DbContext(typeof(PedidosContext))]
    [Migration("20240315144918_AdicionarClienteEmail")]
    partial class AdicionarClienteEmail
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Pedidos.Pedido", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ClienteEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ClienteId")
                        .HasColumnType("uuid");

                    b.Property<int>("Codigo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("Codigo"));

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PedidoStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Pedidos", (string)null);
                });

            modelBuilder.Entity("Domain.Pedidos.PedidoItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("PedidoId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProdutoId")
                        .HasColumnType("uuid");

                    b.Property<string>("ProdutoNome")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<int>("Quantidade")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PedidoId");

                    b.ToTable("PedidoItems", (string)null);
                });

            modelBuilder.Entity("Domain.Pedidos.PedidoItem", b =>
                {
                    b.HasOne("Domain.Pedidos.Pedido", "Pedido")
                        .WithMany("PedidoItems")
                        .HasForeignKey("PedidoId")
                        .IsRequired();

                    b.Navigation("Pedido");
                });

            modelBuilder.Entity("Domain.Pedidos.Pedido", b =>
                {
                    b.Navigation("PedidoItems");
                });
#pragma warning restore 612, 618
        }
    }
}
