using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Arca.Shared.Models; // tu modelo Usuario con UserId, Contrasena, etc.

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

        // IMPORTANTE:
        // - Quitamos el using a Proyecto_2___Paula_Ulate_Medrano.Repositorios
        // - No instanciamos UsuarioRepository (los repos están en la API).
        // - Consumimos la API vía ApiClient (igual que UbicacionController).
        [HttpPost]
        public async Task<ActionResult> Login(string usuario, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
            {
                ViewBag.Mensaje = "Debe ingresar usuario y contraseña.";
                return View("Index");
            }

            using (var api = new ApiClient())
            {
                // Opción A (sin tocar la API): traemos todos y filtramos aquí.
                var usuarios = await api.GetAsync<System.Collections.Generic.List<Usuario>>("api/usuarios")
                               ?? new System.Collections.Generic.List<Usuario>();

                var user = usuarios
                    .FirstOrDefault(u => (u.UserId ?? "").Trim().ToLower() == usuario.Trim().ToLower()
                                      && (u.Contrasena ?? "") == contrasena);

                if (user != null)
                {
                    Session["UsuarioLogueado"] = user;
                    return RedirectToAction("Perfil", "Usuario");
                }
            }

            ViewBag.Mensaje = "Credenciales incorrectas.";
            return View("Index");
        }

        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}