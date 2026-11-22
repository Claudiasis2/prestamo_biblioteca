
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Frontend;
using ProyectoConjuntoMaterialPrestamo.Models;

namespace ProyectoConjuntoMaterialPrestamo.Frontend.Menus
{
    public class MenuPrincipal
    {
        public static async Task Menu(IConsola consola)
        {
            Usuario? usuarioLogueado = null;

            while (true)
            {
                consola.Limpiar();

                consola.Escribir("==================================================\n");
                consola.Escribir(" SISTEMA DE BIBLIOTECA \n");
                consola.Escribir("==================================================\n");

                if (usuarioLogueado != null)
                {
                    consola.Escribir(
                        $" [Sesión activa: {usuarioLogueado.NombreUsuario.Trim()} (ID: {usuarioLogueado.IdUsuario})]\n"
                    );
                    consola.Escribir("--------------------------------------------------\n");
                }
                else
                {
                    Console.WriteLine(" [No hay sesión de usuario activa]");
                    Console.WriteLine("--------------------------------------------------");
                }

                consola.Escribir("1. Menú Administrador\n");
                consola.Escribir("2. Perfil Usuario\n");
                consola.Escribir("3. Iniciar Sesión(Login)\n");
                consola.Escribir("4. Menú Usuario (Solicitudes de Préstamo)\n");
                consola.Escribir("0. SALIR\n");
                consola.Escribir("Opción: ");

                string opcion = consola.LeerTexto();

                if (opcion == "0") break;

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            await MenuAdministrador.Menu(consola);
                            break;

                        case "2":
                            await GestionUsuario.Menu(consola);
                            break;

                        case "3":
                            usuarioLogueado = await GestionLogin.LoginAsync(consola);
                            break;

                        case "4":
                            if (usuarioLogueado == null)
                            {
                                consola.Escribir(
                                    "\n[AVISO] Debe iniciar sesión (Opción 3) antes de acceder a la gestión de solicitudes.\n"
                                );
                                consola.Escribir("Presione cualquier tecla para continuar...\n");
                                consola.EsperarTecla();
                            }
                            else
                            {
                                await GestionSolicitudesPrestamos.Menu(usuarioLogueado.IdUsuario, consola);
                            }
                            break;

                        default:
                            consola.Escribir("Opción no válida.\n");
                            consola.EsperarTecla();
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    consola.Escribir(ex.Message + "\n");
                }
                catch (SqlException ex)
                {
                    consola.Escribir("La operación SQL ha fallado.\n");
                    consola.Escribir(ex.Message + "\n");
                }
                catch (DbUpdateException)
                {
                    consola.Escribir("No se pudieron guardar los cambios.\n");
                }
            }
        }
    }
}
