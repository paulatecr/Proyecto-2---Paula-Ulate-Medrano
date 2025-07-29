using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_2___Paula_Ulate_Medrano.Models
{
    public class Semilla
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
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