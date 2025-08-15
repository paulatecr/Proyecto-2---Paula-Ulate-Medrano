namespace Arca.Shared.Models
{
    public class Especie
    {
        public int Id { get; set; }
        public string NombreCientifico { get; set; } = string.Empty;
        public string NombreComun { get; set; } = string.Empty;
        public string Familia { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }
}