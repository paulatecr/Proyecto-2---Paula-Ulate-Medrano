using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_2___Paula_Ulate_Medrano.Models
{
    public class Especie
    {
        public int Id { get; set; }
        public string NombreCientifico { get; set; }
        public string NombreComun { get; set; }
        public string Familia { get; set; }
        public string Descripcion { get; set; }
    }
}