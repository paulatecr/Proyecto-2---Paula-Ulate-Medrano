/*using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto_2___Paula_Ulate_Medrano.Models.LogicaDatos
{
    public class ConexionBaseDatos
    {
        private readonly string cadenaConexion;

        public ConexionBaseDatos()
        {
            // Lee la cadena desde Web.config
            cadenaConexion = ConfigurationManager.ConnectionStrings["ConexionBaseDatos"].ConnectionString;
        }

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}*/