using Arca.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Proyecto_2___Paula_Ulate_Medrano.Services;

namespace Proyecto_2___Paula_Ulate_Medrano.Controllers
{
    public class UbicacionController : Controller
    {
        // LISTA
        [HttpGet]
        public async Task<ActionResult> Index(string q = null)
        {
            using (var api = new ApiClient())
            {
                var lista = await api.GetAsync<List<Ubicacion>>("api/ubicaciones") ?? new List<Ubicacion>();

                if (!string.IsNullOrWhiteSpace(q))
                {
                    var term = q.Trim().ToLower();
                    lista = lista.Where(u =>
                        (u.Nombre ?? "").ToLower().Contains(term) ||
                        (u.Descripcion ?? "").ToLower().Contains(term) ||
                        (u.Condiciones ?? "").ToLower().Contains(term)
                    ).ToList();
                }

                ViewBag.Filtro = q;
                return View(lista);
            }
        }

        // CREATE
        [HttpGet]
        public ActionResult Create() => View(new Ubicacion());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Ubicacion u)
        {
            if (!ModelState.IsValid) return View(u);

            using (var api = new ApiClient())
            {
                var resp = await api.PostAsync("api/ubicaciones", u);
                if (!resp.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "No se pudo crear la ubicación (API).");
                    return View(u);
                }
            }

            TempData["Mensaje"] = "Ubicación creada correctamente.";
            return RedirectToAction("Index");
        }

        // EDIT
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            using (var api = new ApiClient())
            {
                var model = await api.GetAsync<Ubicacion>($"api/ubicaciones/{id}");
                if (model == null)
                {
                    TempData["Error"] = "Ubicación no encontrada.";
                    return RedirectToAction("Index");
                }
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Ubicacion u)
        {
            if (!ModelState.IsValid) return View(u);

            using (var api = new ApiClient())
            {
                var resp = await api.PutAsync($"api/ubicaciones/{u.Id}", u);
                if (!resp.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "No se pudo actualizar la ubicación (API).");
                    return View(u);
                }
            }

            TempData["Mensaje"] = "Ubicación actualizada correctamente.";
            return RedirectToAction("Index");
        }

        // DELETE
        [HttpGet]
        public async Task<ActionResult> Eliminar(int id)
        {
            using (var api = new ApiClient())
            {
                var resp = await api.DeleteAsync($"api/ubicaciones/{id}");
                if (!resp.IsSuccessStatusCode)
                    TempData["Error"] = "No se pudo eliminar la ubicación (API).";
                else
                    TempData["Mensaje"] = "Ubicación eliminada.";
            }
            return RedirectToAction("Index");
        }
    }
}