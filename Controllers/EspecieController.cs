using Proyecto_2___Paula_Ulate_Medrano.Models;
using Proyecto_2___Paula_Ulate_Medrano.Repositorios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_2___Paula_Ulate_Medrano.Controllers
{
    public class EspecieController : Controller
    {
        private readonly EspecieRepository repositorio;

        public EspecieController()
        {
            var connectionString = ConfigurationManager
                .ConnectionStrings["ConexionBaseDatos"]
                .ConnectionString;

            repositorio = new EspecieRepository(connectionString);
        }

        // GET: Especie
        public ActionResult Index()
        {
            var lista = repositorio.GetAll();
            return View(lista);
        }

        // GET: Especie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Especie/Create
        [HttpPost]
        public ActionResult Create(Especie especie)
        {
            if (ModelState.IsValid)
            {
                repositorio.Insert(especie);
                TempData["Mensaje"] = "Especie Registrada Exitosamente.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Error al Registrar la Especie.";
            return View(especie);
        }

        // GET: Especie/Edit/5
        public ActionResult Edit(int id)
        {
            var especie = repositorio.GetById(id);
            if (especie == null)
            {
                TempData["Error"] = "Especie no Encontrada.";
                return RedirectToAction("Index");
            }

            return View(especie);
        }

        // POST: Especie/Edit/5
        [HttpPost]
        public ActionResult Edit(Especie especie)
        {
            if (ModelState.IsValid)
            {
                repositorio.Update(especie);
                TempData["Mensaje"] = "Especie Actualizada Correctamente.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Error al Actualizar la Especie.";
            return View(especie);
        }

        // GET: Especie/Eliminar/5
        public ActionResult Eliminar(int id)
        {
            var especie = repositorio.GetById(id);
            if (especie == null)
            {
                TempData["Error"] = "Especie no Encontrada.";
            }
            else
            {
                repositorio.Delete(id);
                TempData["Mensaje"] = "Especie Eliminada Correctamente.";
            }

            return RedirectToAction("Index");
        }

        // GET: Especie/Details/5
        public ActionResult Detalle(int id)
        {
            var especie = repositorio.GetById(id);
            if (especie == null)
            {
                TempData["Error"] = "Especie no encontrada.";
                return RedirectToAction("Index");
            }

            ViewBag.EsSoloLectura = true;
            return View("Edit", especie);     
        }
    }

}
