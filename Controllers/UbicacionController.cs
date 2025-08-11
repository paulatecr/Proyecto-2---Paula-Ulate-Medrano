using Proyecto_2___Paula_Ulate_Medrano.Models;
using Proyecto_2___Paula_Ulate_Medrano.Repositorios;
using System.Configuration;
using System.Web.Mvc;

namespace Proyecto_2___Paula_Ulate_Medrano.Controllers
{
    public class UbicacionController : Controller
    {
        private readonly UbicacionRepository repo;

        public UbicacionController()
        {
            var cn = ConfigurationManager.ConnectionStrings["ConexionBaseDatos"].ConnectionString;
            repo = new UbicacionRepository(cn);
        }

        // GET: Ubicacion
        public ActionResult Index()
        {
            var lista = repo.GetAll();
            return View(lista);
        }

        // GET: Ubicacion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ubicacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ubicacion ubicacion)
        {
            if (!ModelState.IsValid) return View(ubicacion);

            repo.Insert(ubicacion);
            TempData["Mensaje"] = "Ubicación creada correctamente.";
            return RedirectToAction("Index");
        }

        // GET: Ubicacion/Edit/5
        public ActionResult Edit(int id)
        {
            var ub = repo.GetById(id);
            if (ub == null)
            {
                TempData["Error"] = "Ubicación no encontrada.";
                return RedirectToAction("Index");
            }
            return View(ub);
        }

        // POST: Ubicacion/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ubicacion ubicacion)
        {
            if (!ModelState.IsValid) return View(ubicacion);

            repo.Update(ubicacion);
            TempData["Mensaje"] = "Ubicación actualizada correctamente.";
            return RedirectToAction("Index");
        }

        // GET: Ubicacion/Eliminar/5
        public ActionResult Eliminar(int id)
        {
            var ub = repo.GetById(id);
            if (ub == null) TempData["Error"] = "Ubicación no encontrada.";
            else
            {
                repo.Delete(id);
                TempData["Mensaje"] = "Ubicación eliminada.";
            }
            return RedirectToAction("Index");
        }
    }
}