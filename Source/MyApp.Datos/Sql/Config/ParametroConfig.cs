using System.Data.Entity.ModelConfiguration;

using MyApp.Entidades;

namespace MyApp.Datos.Sql.Config
{
    public class ParametroConfig : EntityTypeConfiguration<Parametro>
    {
        public ParametroConfig()
        {
            ToTable(Entidades.Metadata.ParametroMetadata.BD.TABLA);
            HasKey(p => p.Nombre);

            Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(Entidades.Metadata.ParametroMetadata.Propiedades.Nombre.TAMAÑO_MAX);
        }
    }
}
