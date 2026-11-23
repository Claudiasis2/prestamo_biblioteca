
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    static class GestionLogin
    {
        public static async Task<Usuario?> LoginAsync(IConsola consola)
        {
            consola.Limpiar();

            consola.Escribir("==================================================\n");
            consola.Escribir("                   INICIO DE SESIÓN               \n");
            consola.Escribir("==================================================\n");

            consola.Escribir("Email: ");
            string email = consola.LeerTexto();

            consola.Escribir("Password: ");
            string password = consola.LeerTexto();

            var usuario = await LoginServicio.LoginAsync(email, password);

            if (usuario != null)
            {
                consola.Escribir($"\n¡BIENVENIDO/A {usuario.NombreUsuario.Trim()}!\n");
                SalirPantalla(consola);
                return usuario;
            }
            else
            {
                consola.Escribir("\n[ERROR] Credenciales incorrectas o usuario inactivo.\n");
                SalirPantalla(consola);
                return null;
            }
        }

        public static void SalirPantalla(IConsola consola)
        {
            consola.Escribir("\nPulsa una tecla para continuar...\n");
            consola.EsperarTecla();
            consola.Limpiar();
        }
    }
}
