using Arca.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Proyecto_2___Paula_Ulate_Medrano.Services;

namespace Proyecto_2___Paula_Ulate_Medrano.Controllers
{
    public class EspecieController : Controller
    {
        // =======================================
        // INDEX 
        // =======================================
        [HttpGet]
        public async Task<ActionResult> Index(string q = null)
        {
            using (var api = new ApiClient())
            {
                var especies = await api.GetAsync<List<Especie>>("api/especies")
                               ?? new List<Especie>();

                if (!string.IsNullOrWhiteSpace(q))
                {
                    var term = q.Trim().ToLower();
                    especies = especies
                        .Where(e =>
                            (e.NombreComun ?? "").ToLower().Contains(term) ||
                            (e.NombreCientifico ?? "").ToLower().Contains(term) ||
                            (e.Familia ?? "").ToLower().Contains(term))
                        .ToList();
                }

                ViewBag.Filtro = q;
                return View(especies);
            }
        }

        // =======================================
        // CREAR
        // =======================================
        [HttpGet]
        public ActionResult Create()
        {
            return View(new Especie());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Especie e)
        {
            if (!ModelState.IsValid)
                return View(e);

            using (var api = new ApiClient())
            {
                var resp = await api.PostAsync("api/especies", e);
                if (!resp.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "No se pudo crear la especie (API).");
                    return View(e);
                }
            }

            TempData["Mensaje"] = "Especie creada correctamente.";
            return RedirectToAction("Index");
        }

        // =======================================
        // EDITAR
        // =======================================
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            using (var api = new ApiClient())
            {
                var especie = await api.GetAsync<Especie>($"api/especies/{id}");
                if (especie == null)
                {
                    TempData["Error"] = "Especie no encontrada.";
                    return RedirectToAction("Index");
                }
                return View(especie);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Especie e)
        {
            if (!ModelState.IsValid)
                return View(e);

            using (var api = new ApiClient())
            {
                var resp = await api.PutAsync($"api/especies/{e.Id}", e);
                if (!resp.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "No se pudo actualizar la especie (API).");
                    return View(e);
                }
            }

            TempData["Mensaje"] = "Especie actualizada correctamente.";
            return RedirectToAction("Index");
        }

        // =======================================
        // ELIMINAR 
        // =======================================
        [HttpGet]
        public async Task<ActionResult> Eliminar(int id)
        {
            using (var api = new ApiClient())
            {
                var resp = await api.DeleteAsync($"api/especies/{id}");
                if (!resp.IsSuccessStatusCode)
                    TempData["Error"] = "No se pudo eliminar la especie (API).";
                else
                    TempData["Mensaje"] = "Especie eliminada correctamente.";
            }
            return RedirectToAction("Index");
        }

        // =======================================
        // DETALLE 
        // =======================================
        [HttpGet]
        public async Task<ActionResult> Detalle(int id)
        {
            using (var api = new ApiClient())
            {
                var especie = await api.GetAsync<Especie>($"api/especies/{id}");
                if (especie == null)
                {
                    TempData["Error"] = "Especie no encontrada.";
                    return RedirectToAction("Index");
                }
                return View(especie); // Crea una vista Detalle si la usarás
            }
        }
    }
}