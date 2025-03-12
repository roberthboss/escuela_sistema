using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

public class AuthController : Controller
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /Auth/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: /Auth/Register
    [HttpPost]
    public async Task<IActionResult> Register(string usuario, string password)
    {
        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError("", "El usuario y la contraseña son obligatorios.");
            return View();
        }

        bool userExists = await _context.usuarios.AnyAsync(u => u.usuario == usuario);
        if (userExists)
        {
            ModelState.AddModelError("", "Ese usuario ya existe.");
            return View();
        }

        var newUser = new Usuario
        {
            usuario = usuario,
            password = password
        };

        _context.usuarios.Add(newUser);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Login));
    }

    // GET: /Auth/Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // POST: /Auth/Login
    [HttpPost]
    public async Task<IActionResult> Login(string usuario, string password)
    {
        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError("", "El usuario y la contraseña son obligatorios.");
            return View();
        }

        var user = await _context.usuarios.FirstOrDefaultAsync(u => u.usuario == usuario);
        if (user == null || user.password != password)
        {
            ModelState.AddModelError("", "Credenciales inválidas.");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.usuario),
            new Claim("UserId", user.user_id.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

        return RedirectToAction("Index", "Home");
    }

    // GET: /Auth/Logout
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }
}
