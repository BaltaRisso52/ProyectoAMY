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
        ViewData["IsLoginPage"] = true;
        var model = new LoginViewModel
        {
            Username = HttpContext.Session.GetString("User"),
            IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "true"
        };
        return View(model);
    }

    [HttpPost]
    public ActionResult Login(LoginViewModel model)
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

    public IActionResult Logout()
    {
        // Limpiar la sesión
        HttpContext.Session.Clear();

        // Redirigir a la vista de login
        return RedirectToAction("Index");
    }
}