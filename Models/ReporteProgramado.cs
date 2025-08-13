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
        public string Frecuencia { get; set; } 
        public string Parametros { get; set; } 
        public DateTime ProximoEnvio { get; set; }
        public string Destinatarios { get; set; } 
        public DateTime FechaCreacion { get; set; }
        public int CreadoPor { get; set; }
    }
}