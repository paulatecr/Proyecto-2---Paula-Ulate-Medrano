using Arca.Shared.Models;
using Proyecto_2___Paula_Ulate_Medrano.Repositorios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_2___Paula_Ulate_Medrano.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepository repositorio;

        public UsuarioController()
        {
            var connectionString = ConfigurationManager
                .ConnectionStrings["ConexionBaseDatos"]
                .ConnectionString;

            repositorio = new UsuarioRepository(connectionString);
        }
        public ActionResult Index()
        {
            var listaUsuarios = repositorio.GetAll();
            return View(listaUsuarios);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                repositorio.Insert(usuario);
                return RedirectToAction("Index");
            }

            return View(usuario);
        }

        public ActionResult Perfil()
        {
            var user = Session["UsuarioLogueado"] as Usuario;
            if (user == null)
                return RedirectToAction("Index", "Home");

            return View(user);
        }
    }
}