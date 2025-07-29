using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_2___Paula_Ulate_Medrano.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string ContrasenaHash { get; set; }
        public string Rol { get; set; }
        public bool Activo { get; set; }
    }

}