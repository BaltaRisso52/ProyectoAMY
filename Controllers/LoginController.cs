using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private IUsuarioRepository usuarioRepository;

    public LoginController(IUsuarioRepository usuarioRepository)
    {
        this.usuarioRepository = usuarioRepository;
    }

    public ActionResult Index()
    {
        try
        {
            ViewData["IsLoginPage"] = true;
            var model = new LoginViewModel
            {
                Username = HttpContext.Session.GetString("User"),
                IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "true"
            };
            return View(model);

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    [HttpPost]
    public ActionResult Login(LoginViewModel model)
    {

        try
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                model.ErrorMessage = "Por favor ingrese su nombre de usuario y contraseña.";
                return RedirectToAction("Index");
            }

            Usuarios? usuario = usuarioRepository.GetUser(model.Username, model.Password);

            if (usuario != null)
            {
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("User", usuario.NombreUsuario);
                HttpContext.Session.SetInt32("IdUser", usuario.Id);

                return RedirectToAction("Index", "Productos");
            }


            model.ErrorMessage = "Credenciales Inválidas";
            model.IsAuthenticated = false;
            return View("Index", model);

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    public IActionResult Logout()
    {

        try
        {
            // Limpiar la sesión
            HttpContext.Session.Clear();

            // Redirigir a la vista de login
            return RedirectToAction("Index");

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }
}