using SIRESU.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SIRESU.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SiresuContext context)
        {
            context.Database.EnsureCreated();

            if (context.Usuarios.Any())
            {
                // La base ya tiene datos
                return;
            }

            var admin = new Usuario
            {
                Nombre = "Administrador",
                Correo = "admin@ejemplo.com",
                Contraseña = ObtenerHashSHA256("admin123"),
                Rol = "administrador"
            };

            var cliente = new Usuario
            {
                Nombre = "Cliente",
                Correo = "cliente@ejemplo.com",
                Contraseña = ObtenerHashSHA256("cliente123"),
                Rol = "cliente"
            };

            context.Usuarios.Add(admin);
            context.Usuarios.Add(cliente);
            context.SaveChanges();
        }

        private static string ObtenerHashSHA256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}
