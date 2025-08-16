using Arca.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Proyecto_2___Paula_Ulate_Medrano.Controllers
{
    public class UsuarioController : Controller
    {
        // LISTA
        [HttpGet]
        public async Task<ActionResult> Index(string q = null)
        {
            using (var api = new ApiClient())
            {
                var usuarios = await api.GetAsync<List<Usuario>>("api/usuarios") ?? new List<Usuario>();

                if (!string.IsNullOrWhiteSpace(q))
                {
                    var term = q.Trim().ToLower();
                    usuarios = usuarios.Where(u =>
                        (u.UserId ?? "").ToLower().Contains(term) ||
                        (u.Nombre ?? "").ToLower().Contains(term) ||
                        (u.Rol ?? "").ToLower().Contains(term) ||
                        (u.Correo ?? "").ToLower().Contains(term)
                    ).ToList();
                }

                ViewBag.Filtro = q;
                return View(usuarios);
            }
        }

        // CREAR
        [HttpGet]
        public ActionResult Create()
        {
            return View(new Usuario { Activo = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Usuario usuario)
        {
            if (!ModelState.IsValid) return View(usuario);

            using (var api = new ApiClient())
            {
                var resp = await api.PostAsync("api/usuarios", usuario);
                if (!resp.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "No se pudo crear el usuario (API).");
                    return View(usuario);
                }
            }

            TempData["Mensaje"] = "Usuario creado correctamente.";
            return RedirectToAction("Index");
        }

        // EDITAR
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            using (var api = new ApiClient())
            {
                var model = await api.GetAsync<Usuario>($"api/usuarios/{id}");
                if (model == null)
                {
                    TempData["Error"] = "Usuario no encontrado.";
                    return RedirectToAction("Index");
                }
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Usuario usuario)
        {
            if (!ModelState.IsValid) return View(usuario);

            using (var api = new ApiClient())
            {
                var resp = await api.PutAsync($"api/usuarios/{usuario.Id}", usuario);
                if (!resp.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "No se pudo actualizar el usuario (API).");
                    return View(usuario);
                }
            }

            TempData["Mensaje"] = "Usuario actualizado correctamente.";
            return RedirectToAction("Index");
        }

        // ELIMINAR (integrado con tu modal por GET)
        [HttpGet]
        public async Task<ActionResult> Eliminar(int id)
        {
            using (var api = new ApiClient())
            {
                var resp = await api.DeleteAsync($"api/usuarios/{id}");
                if (!resp.IsSuccessStatusCode)
                    TempData["Error"] = "No se pudo eliminar el usuario (API).";
                else
                    TempData["Mensaje"] = "Usuario eliminado correctamente.";
            }
            return RedirectToAction("Index");
        }

        // PERFIL (lee de Session)
        [HttpGet]
        public ActionResult Perfil()
        {
            var user = Session["UsuarioLogueado"] as Usuario;
            if (user == null)
                return RedirectToAction("Index", "Home");

            return View(user);
        }
    }
}