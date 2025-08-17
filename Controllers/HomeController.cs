using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Proyecto_2___Paula_Ulate_Medrano.Services;
using Arca.Shared.Models;

namespace Proyecto_2___Paula_Ulate_Medrano.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() => View();

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

        // DTO para postear al API
        public class LoginRequest { public string userId { get; set; } public string contrasena { get; set; } }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string userId, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(contrasena))
            {
                ViewBag.Mensaje = "Debe ingresar UserID y contraseña.";
                return View("Index");
            }

            using (var api = new ApiClient())
            {
                var body = new LoginRequest { userId = userId, contrasena = contrasena };
                var resp = await api.PostAsync("api/usuarios/login", body);

                if (!resp.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Credenciales incorrectas o API no disponible.";
                    return View("Index");
                }

                var json = await resp.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<Usuario>(json);
                if (user == null)
                {
                    ViewBag.Mensaje = "No se pudo autenticar al usuario.";
                    return View("Index");
                }

                Session["UsuarioLogueado"] = user;

                return RedirectToAction("Perfil", "Usuario");
            }
        }

        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}