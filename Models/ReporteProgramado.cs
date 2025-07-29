using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_2___Paula_Ulate_Medrano.Models
{
    public class ReporteProgramado
    {
        public int Id { get; set; }
        public string NombreReporte { get; set; }
        public string Frecuencia { get; set; } // Diario, Semanal, etc.
        public string Parametros { get; set; } // JSON u otro formato
        public DateTime ProximoEnvio { get; set; }
        public string Destinatarios { get; set; } // Correos separados por coma
        public DateTime FechaCreacion { get; set; }
        public int CreadoPor { get; set; }
    }
}