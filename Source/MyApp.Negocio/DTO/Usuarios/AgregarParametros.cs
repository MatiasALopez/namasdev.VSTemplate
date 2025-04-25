namespace MyApp.Negocio.DTO.Usuarios
{
    public class AgregarParametros : ParametrosEntidadBase<string>
    {
        public string Email { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
    }
}
