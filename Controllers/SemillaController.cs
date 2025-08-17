using Arca.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Proyecto_2___Paula_Ulate_Medrano.Services;

namespace Proyecto_2___Paula_Ulate_Medrano.Controllers
{
    public class SemillaController : Controller
    {
        // =========================
        // LISTADO
        // =========================
        [HttpGet]
        public async Task<ActionResult> Index(int? especieId)
        {
            using (var api = new ApiClient())
            {
                var semillas = await api.GetAsync<List<Semilla>>("api/semillas");
                var especies = await api.GetAsync<List<Especie>>("api/especies");

                ViewBag.Especies = new SelectList(especies ?? new List<Especie>(), "Id", "NombreComun", especieId);
                ViewBag.EspecieIdSeleccionada = especieId;

                if (especieId.HasValue)
                    semillas = (semillas ?? new List<Semilla>())
                               .Where(s => s.EspecieId == especieId.Value)
                               .ToList();

                return View(semillas ?? new List<Semilla>());
            }
        }

        // =========================
        // CREAR
        // =========================
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            await CargarCombosAsync();
            return View(new Semilla
            {
                FechaAlmacenamiento = DateTime.Now,
                Cantidad = 0
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Semilla semilla)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombosAsync(semilla.EspecieId, semilla.UbicacionId);
                return View(semilla);
            }

            // Trazabilidad mínima
            semilla.FechaCreacion = DateTime.Now;
            semilla.CreadoPor = (Session["UsuarioLogueado"] as Usuario)?.Id ?? 1;

            using (var api = new ApiClient())
            {
                var resp = await api.PostAsync("api/semillas", semilla);
                if (!resp.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "No se pudo crear la semilla (API).");
                    await CargarCombosAsync(semilla.EspecieId, semilla.UbicacionId);
                    return View(semilla);
                }
            }

            TempData["Mensaje"] = "Semilla agregada correctamente.";
            return RedirectToAction("Index");
        }

        // =========================
        // EDITAR
        // =========================
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            using (var api = new ApiClient())
            {
                var semilla = await api.GetAsync<Semilla>($"api/semillas/{id}");
                if (semilla == null)
                {
                    TempData["Error"] = "Semilla no encontrada.";
                    return RedirectToAction("Index");
                }
                await CargarCombosAsync(semilla.EspecieId, semilla.UbicacionId);
                return View(semilla);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Semilla semilla)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombosAsync(semilla.EspecieId, semilla.UbicacionId);
                return View(semilla);
            }

            semilla.FechaModificacion = DateTime.Now;
            semilla.ModificadoPor = (Session["UsuarioLogueado"] as Usuario)?.Id ?? 1;

            using (var api = new ApiClient())
            {
                var resp = await api.PutAsync($"api/semillas/{semilla.Id}", semilla);
                if (!resp.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "No se pudo actualizar la semilla (API).");
                    await CargarCombosAsync(semilla.EspecieId, semilla.UbicacionId);
                    return View(semilla);
                }
            }

            TempData["Mensaje"] = "Semilla actualizada correctamente.";
            return RedirectToAction("Index");
        }

        // =========================
        // ELIMINAR 
        // =========================
        [HttpGet]
        public async Task<ActionResult> Eliminar(int id)
        {
            using (var api = new ApiClient())
            {
                var resp = await api.DeleteAsync($"api/semillas/{id}");
                if (!resp.IsSuccessStatusCode)
                    TempData["Error"] = "No se pudo eliminar la semilla (API).";
                else
                    TempData["Mensaje"] = "Semilla eliminada correctamente.";
            }
            return RedirectToAction("Index");
        }

        // =========================
        // INVENTARIO
        // =========================
        [HttpGet]
        public async Task<ActionResult> Inventario()
        {
            using (var api = new ApiClient())
            {
                var lista = await api.GetAsync<List<SemillaGrid>>("api/semillas/grid");
                return View(lista ?? new List<SemillaGrid>());
            }
        }

        // =========================
        // DETALLE 
        // =========================
        [HttpGet]
        public async Task<ActionResult> Detalle(int id)
        {
            using (var api = new ApiClient())
            {
                var s = await api.GetAsync<Semilla>($"api/semillas/{id}");
                if (s == null)
                {
                    TempData["Error"] = "Semilla no encontrada.";
                    return RedirectToAction("Inventario");
                }

                var especies = await api.GetAsync<List<Especie>>("api/especies");
                var ubicaciones = await api.GetAsync<List<Ubicacion>>("api/ubicaciones");

                var e = especies?.FirstOrDefault(x => x.Id == s.EspecieId);
                var u = ubicaciones?.FirstOrDefault(x => x.Id == s.UbicacionId);

                ViewBag.NombreEspecie = e?.NombreComun ?? e?.NombreCientifico ?? $"Especie #{s.EspecieId}";
                ViewBag.NombreUbicacion = u?.Nombre ?? $"Ubicación #{s.UbicacionId}";

                return View("Detalle", s);
            }
        }

        // =========================
        // Helpers
        // =========================
        private async Task CargarCombosAsync(int? especieId = null, int? ubicacionId = null)
        {
            using (var api = new ApiClient())
            {
                var especies = await api.GetAsync<List<Especie>>("api/especies");
                var ubicaciones = await api.GetAsync<List<Ubicacion>>("api/ubicaciones");

                ViewBag.Especies = new SelectList(especies ?? new List<Especie>(), "Id", "NombreComun", especieId);
                ViewBag.Ubicaciones = new SelectList(ubicaciones ?? new List<Ubicacion>(), "Id", "Nombre", ubicacionId);
            }
        }
    }
}