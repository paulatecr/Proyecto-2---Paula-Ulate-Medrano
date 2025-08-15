using System;

namespace Arca.Shared.Models
{
    public class ReporteProgramado
    {
        public int Id { get; set; }
        public string NombreReporte { get; set; } = string.Empty;
        public string Frecuencia { get; set; } = string.Empty;
        public string Parametros { get; set; } = string.Empty;
        public DateTime ProximoEnvio { get; set; }
        public string Destinatarios { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public int CreadoPor { get; set; }
    }
}