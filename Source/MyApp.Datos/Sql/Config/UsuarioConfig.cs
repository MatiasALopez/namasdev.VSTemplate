using System.ComponentModel.DataAnnotations.Schema;

using MyApp.Entidades;
using namasdev.Data.Entity.Config;

namespace MyApp.Datos.Sql.Config
{
    public class UsuarioConfig : EntidadBorradoConfigBase<Usuario>
    {
        public UsuarioConfig()
        {
            ToTable(Entidades.Metadata.UsuarioMetadata.BD.TABLA);
            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasColumnName(Entidades.Metadata.UsuarioMetadata.BD.TABLA_ID)
                .IsRequired()
                .HasMaxLength(Entidades.Metadata.UsuarioMetadata.UsuarioId.TAMAÑO_MAX);

            Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(Entidades.Metadata.UsuarioMetadata.Email.TAMAÑO_MAX);

            Property(e => e.Nombres)
                .IsRequired()
                .HasMaxLength(Entidades.Metadata.UsuarioMetadata.Nombres.TAMAÑO_MAX);

            Property(e => e.Apellidos)
                .IsRequired()
                .HasMaxLength(Entidades.Metadata.UsuarioMetadata.Apellidos.TAMAÑO_MAX);

            Property(e => e.ApellidosYNombres)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            Property(e => e.NombresYApellidos)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            HasMany(s => s.Roles)
                .WithMany()
                .Map(cs =>
                {
                    cs.MapLeftKey("UserId");
                    cs.MapRightKey("RoleId");
                    cs.ToTable("AspNetUserRoles");
                });
        }
    }
}
