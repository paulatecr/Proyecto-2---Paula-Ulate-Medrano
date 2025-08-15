using System;

namespace Arca.Shared.Models
{
    public class Semilla
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int EspecieId { get; set; }
        public int UbicacionId { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaAlmacenamiento { get; set; }

        // Trazabilidad
        public DateTime FechaCreacion { get; set; }
        public int CreadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? ModificadoPor { get; set; }
    }
}