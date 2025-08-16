using System;
namespace Arca.Shared.Models
{
    public class SemillaGrid
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int EspecieId { get; set; }
        public int UbicacionId { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaAlmacenamiento { get; set; }
        public string NombreEspecie { get; set; } = string.Empty;
        public string NombreUbicacion { get; set; } = string.Empty;
    }
}