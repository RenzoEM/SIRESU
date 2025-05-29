using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRESU.Data;
using SIRESU.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SIRESU.Controllers
{
    public class CuentaController : Controller
    {
        private readonly SiresuContext _context;

        public CuentaController(SiresuContext context)
        {
            _context = context;
        }

        // GET: /Cuenta/Login
        public IActionResult Login() => View();

        // POST: /Cuenta/Login
        [HttpPost]
        public async Task<IActionResult> Login(string correo, string contraseña)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contraseña))
            {
                ViewBag.Error = "Todos los campos son obligatorios.";
                return View();
            }

            string hash = ObtenerHashSHA256(contraseña);

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Contraseña == hash);

            if (usuario == null)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View();
            }

            HttpContext.Session.SetString("Usuario", usuario.Nombre);
            HttpContext.Session.SetString("Rol", usuario.Rol);

            return usuario.Rol.ToLower() switch
            {
                "administrador" => RedirectToAction("Panel", "Admin"),
                "cliente" => RedirectToAction("Inicio", "Cliente"),
                _ => RedirectToAction("Login")
            };
        }

        // GET: /Cuenta/Registro
        public IActionResult Registro() => View();

        // POST: /Cuenta/Registro
        [HttpPost]
        public async Task<IActionResult> Registro(string nombre, string correo, string contraseña)
        {
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contraseña))
            {
                ViewBag.Error = "Todos los campos son obligatorios.";
                return View();
            }

            if (await _context.Usuarios.AnyAsync(u => u.Correo == correo))
            {
                ViewBag.Error = "Ya existe un usuario con ese correo.";
                return View();
            }

            string hash = ObtenerHashSHA256(contraseña);

            var nuevoUsuario = new Usuario
            {
                Nombre = nombre,
                Correo = correo,
                Contraseña = hash,
                Rol = "cliente"
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            ViewBag.Exito = "Registro exitoso. Ya puedes iniciar sesión.";
            return RedirectToAction("Login");
        }

        private string ObtenerHashSHA256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}
