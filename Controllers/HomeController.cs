using Proyecto_2___Paula_Ulate_Medrano.Repositorios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto_2___Paula_Ulate_Medrano.Services;

namespace Proyecto_2___Paula_Ulate_Medrano.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Login(string usuario, string contrasena)
        {
            var repo = new UsuarioRepository(ConfigurationManager.ConnectionStrings["ConexionBaseDatos"].ConnectionString);
            var usuarios = repo.GetAll();
            var user = usuarios.FirstOrDefault(u => u.UserID == usuario && u.Contrasena == contrasena);

            if (user != null)
            {
                Session["UsuarioLogueado"] = user;
                return RedirectToAction("Perfil", "Usuario");
            }
            else
            {
                ViewBag.Mensaje = "Credenciales incorrectas.";
                return View("Index");
            }
        }

        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }



    }
}