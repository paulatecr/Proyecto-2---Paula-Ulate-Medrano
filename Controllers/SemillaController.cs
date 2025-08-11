using Proyecto_2___Paula_Ulate_Medrano.Models;
using Proyecto_2___Paula_Ulate_Medrano.Repositorios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace Proyecto_2___Paula_Ulate_Medrano.Controllers
{
    public class SemillaController : Controller
    {
        private readonly SemillaRepository repoSemilla;
        private readonly EspecieRepository repoEspecie;
        private readonly UbicacionRepository repoUbicacion;

        public SemillaController()
        {
            var cn = ConfigurationManager.ConnectionStrings["ConexionBaseDatos"].ConnectionString;
            repoSemilla = new SemillaRepository(cn);
            repoEspecie = new EspecieRepository(cn);
            repoUbicacion = new UbicacionRepository(cn);
        }

        // GET: Semilla
        public ActionResult Index(int? especieId)
        {
            var semillas = repoSemilla.GetAll(); // trae todas
            ViewBag.Especies = new SelectList(repoEspecie.GetAll(), "Id", "NombreComun");
            ViewBag.EspecieIdSeleccionada = especieId;

            if (especieId.HasValue)
                semillas = semillas.FindAll(s => s.EspecieId == especieId.Value);

            return View(semillas);
        }

        // GET: Semilla/Create
        public ActionResult Create()
        {
            CargarCombos();
            return View(new Semilla { FechaAlmacenamiento = DateTime.Now, Cantidad = 0 });
        }

        // POST: Semilla/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Semilla semilla)
        {
            if (!ModelState.IsValid)
            {
                CargarCombos();
                return View(semilla);
            }

            // trazabilidad mínima (ajusta con tu sesión)
            semilla.FechaCreacion = DateTime.Now;
            semilla.CreadoPor = 1; // TODO: reemplazar por (Session["UsuarioLogueado"] as Usuario).Id

            repoSemilla.Insert(semilla);
            TempData["Mensaje"] = "Semilla agregada correctamente.";
            return RedirectToAction("Index");
        }

        // GET: Semilla/Edit/5
        public ActionResult Edit(int id)
        {
            var semilla = repoSemilla.GetById(id);
            if (semilla == null)
            {
                TempData["Error"] = "Semilla no encontrada.";
                return RedirectToAction("Index");
            }
            CargarCombos();
            return View(semilla);
        }

        // POST: Semilla/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Semilla semilla)
        {
            if (!ModelState.IsValid)
            {
                CargarCombos();
                return View(semilla);
            }

            semilla.FechaModificacion = DateTime.Now;
            semilla.ModificadoPor = 1; // TODO: usuario sesión

            repoSemilla.Update(semilla);
            TempData["Mensaje"] = "Semilla actualizada correctamente.";
            return RedirectToAction("Index");
        }

        // GET: Semilla/Eliminar/5
        public ActionResult Eliminar(int id)
        {
            var s = repoSemilla.GetById(id);
            if (s == null)
            {
                TempData["Error"] = "Semilla no encontrada.";
            }
            else
            {
                repoSemilla.Delete(id);
                TempData["Mensaje"] = "Semilla eliminada correctamente.";
            }
            return RedirectToAction("Index");
        }

        private void CargarCombos()
        {
            ViewBag.Especies = new SelectList(repoEspecie.GetAll(), "Id", "NombreComun");
            ViewBag.Ubicaciones = new SelectList(repoUbicacion.GetAll(), "Id", "Nombre");
        }
    }
}