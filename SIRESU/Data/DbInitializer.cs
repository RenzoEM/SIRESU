using SIRESU.Models;
using System.Security.Cryptography;
using System.Text;

namespace SIRESU.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SiresuContext context)
        {
            context.Database.EnsureCreated();

            // Si ya hay usuarios, no hacer nada
            if (context.Usuarios.Any()) return;

            string Hash(string input)
            {
                using var sha = SHA256.Create();
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }

            var usuarios = new Usuario[]
            {
                new Usuario
                {
                    Nombre = "Admin Principal",
                    Correo = "admin@siresu.com",
                    Contraseña = Hash("admin123"),
                    Rol = "administrador"
                },
                new Usuario
                {
                    Nombre = "Cliente Demo",
                    Correo = "cliente@siresu.com",
                    Contraseña = Hash("cliente123"),
                    Rol = "cliente"
                }
            };

            foreach (var u in usuarios)
            {
                context.Usuarios.Add(u);
            }

            context.SaveChanges();
        }
    }
}
