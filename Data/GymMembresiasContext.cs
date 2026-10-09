using System;
using System.Collections.Generic;
using GymMembershipApiV2.Models;
using Microsoft.EntityFrameworkCore;

namespace GymMembershipApiV2.Data;

public partial class GymMembresiasContext : DbContext
{
    public GymMembresiasContext()
    {
    }

    public GymMembresiasContext(DbContextOptions<GymMembresiasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asistencia> Asistencias { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Membresia> Membresias { get; set; }

    public virtual DbSet<MetodosPago> MetodosPagos { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<PlanesMembresium> PlanesMembresia { get; set; }

    public virtual DbSet<Socio> Socios { get; set; }



    public virtual DbSet<Usuario> Usuarios { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\GYMSERVER;Database=GymMembresias;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asistencia>(entity =>
        {
            entity.HasKey(e => e.AsistenciaId).HasName("PK__Asistenc__72710F45CF230199");

            entity.HasIndex(e => new { e.SocioId, e.FechaHoraEntrada }, "IX_Asistencias_Socio_Fecha");

            entity.Property(e => e.AsistenciaId).HasColumnName("AsistenciaID");
            entity.Property(e => e.FechaHoraEntrada).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.SocioId).HasColumnName("SocioID");

            entity.HasOne(d => d.Socio).WithMany(p => p.Asistencia)
                .HasForeignKey(d => d.SocioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asistencias_Socios");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.EmpleadoId).HasName("PK__Empleado__958BE6F0D4DA6CF8");

            entity.HasIndex(e => e.Documento, "UQ__Empleado__AF73706DD26729E0").IsUnique();

            entity.Property(e => e.EmpleadoId).HasColumnName("EmpleadoID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellidos).HasMaxLength(100);
            entity.Property(e => e.Cargo).HasMaxLength(50);
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.Documento).HasMaxLength(30);
            entity.Property(e => e.FechaContrato).HasDefaultValueSql("(CONVERT([date],sysdatetime()))");
            entity.Property(e => e.Nombres).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Membresia>(entity =>
        {
            entity.HasKey(e => e.MembresiaId).HasName("PK__Membresi__5AE930776E3B0D88");

            entity.HasIndex(e => e.SocioId, "IX_Membresias_Socio");

            entity.HasIndex(e => new { e.FechaVencimiento, e.Estado }, "IX_Membresias_Vencimiento");

            entity.Property(e => e.MembresiaId).HasColumnName("MembresiaID");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("Activa");
            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.SocioId).HasColumnName("SocioID");

            entity.HasOne(d => d.Plan).WithMany(p => p.Membresia)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Membresias_Planes");

            entity.HasOne(d => d.Socio).WithMany(p => p.Membresia)
                .HasForeignKey(d => d.SocioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Membresias_Socios");
        });

        modelBuilder.Entity<MetodosPago>(entity =>
        {
            entity.HasKey(e => e.MetodoPagoId).HasName("PK__MetodosP__A8FEAF7499B8C965");

            entity.ToTable("MetodosPago");

            entity.HasIndex(e => e.Nombre, "UQ__MetodosP__75E3EFCF140E4041").IsUnique();

            entity.Property(e => e.MetodoPagoId).HasColumnName("MetodoPagoID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PK__Pagos__F00B6158D4C57F7B");

            entity.HasIndex(e => e.MembresiaId, "IX_Pagos_Membresia");

            entity.Property(e => e.PagoId).HasColumnName("PagoID");
            entity.Property(e => e.FechaPago).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.MembresiaId).HasColumnName("MembresiaID");
            entity.Property(e => e.MetodoPagoId).HasColumnName("MetodoPagoID");
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Referencia).HasMaxLength(100);

            entity.HasOne(d => d.Membresia).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.MembresiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pagos_Membresias");

            entity.HasOne(d => d.MetodoPago).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.MetodoPagoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pagos_MetodosPago");

            entity.HasOne(d => d.RegistradoPorNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.RegistradoPor)
                .HasConstraintName("FK_Pagos_Empleados");
        });

        modelBuilder.Entity<PlanesMembresium>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PK__PlanesMe__755C22D74215D687");

            entity.HasIndex(e => e.Nombre, "UQ__PlanesMe__75E3EFCF60A0DEC9").IsUnique();

            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Socio>(entity =>
        {
            entity.HasKey(e => e.SocioId).HasName("PK__Socios__165D08DA6964A683");

            entity.HasIndex(e => e.Documento, "UQ__Socios__AF73706D9B9CCCF6").IsUnique();

            entity.Property(e => e.SocioId).HasColumnName("SocioID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellidos).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.Documento).HasMaxLength(30);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Nombres).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7989784AA98");

            entity.HasIndex(e => e.NombreUsuario, "UQ__Usuarios__6B0F5AE0DA43E154").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.ContrasenaHash).HasMaxLength(255);
            entity.Property(e => e.EmpleadoId).HasColumnName("EmpleadoID");
            entity.Property(e => e.NombreUsuario).HasMaxLength(50);
            entity.Property(e => e.Rol).HasMaxLength(30);
            entity.Property(e => e.SocioId).HasColumnName("SocioID");

            entity.HasOne(d => d.Empleado).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.EmpleadoId)
                .HasConstraintName("FK_Usuarios_Empleados");

            entity.HasOne(d => d.Socio).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.SocioId)
                .HasConstraintName("FK_Usuarios_Socios");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
