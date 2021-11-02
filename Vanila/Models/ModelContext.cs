using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Vanila.Models
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Detail> Details { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1522))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));Persist Security Info=True;User Id=vanila;Password=oracle;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("VANILA")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DId);

                entity.ToTable("DEPARTMENT");

                entity.Property(e => e.DId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("DID");

                entity.Property(e => e.DName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DNAME");
            });

            modelBuilder.Entity<Detail>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("DETAILS");

                entity.Property(e => e.DId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("DID");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
