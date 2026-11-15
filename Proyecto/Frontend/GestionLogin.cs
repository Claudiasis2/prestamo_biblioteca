using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;
using System;
using System.Threading.Tasks;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    static class GestionLogin
    {
        public static async Task<Usuario?> LoginAsync()
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine("                   INICIO DE SESIÓN               ");
            Console.WriteLine("==================================================");
            string Email = LeerTexto("Email: ");
            string Password = LeerTexto("Password: ");

            var usuario = await LoginServicio.LoginAsync(Email, Password);

            if (usuario != null)
            {
                Console.WriteLine($"\n¡BIENVENIDO/A {usuario.NombreUsuario.Trim()}!");
                SalirPantalla();
                return usuario;
            }
            else
            {
                Console.WriteLine("\n[ERROR] Credenciales incorrectas o usuario inactivo.");
                SalirPantalla();
                return null;
            }
        }

        static string LeerTexto(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine()?.Trim() ?? "";
        }

        public static void SalirPantalla()
        {
            Console.WriteLine("\nPulsa una tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}