
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;

namespace ProyectoConjuntoMaterialPrestamo.Frontend.Menus
{
    internal class MenuPrincipal
    {
        public static Usuario? usuarioLogueado = null;

        public static async Task Menu(IConsola consola)
        {
            while (true)
            {
                consola.Limpiar();

                consola.Escribir("==================================================\n");
                consola.Escribir(" SISTEMA DE BIBLIOTECA \n");
                consola.Escribir("==================================================\n");

                if (usuarioLogueado != null)
                {
                    consola.Escribir(
                        $" [Sesión Usuario activa: {usuarioLogueado.NombreUsuario.Trim()} (ID: {usuarioLogueado.IdUsuario})]\n"
                    );

                    consola.Escribir("--------------------------------------------------\n");
                }
                else
                {
                    consola.Escribir(" [No hay sesión de usuario activa]\n");
                    consola.Escribir("--------------------------------------------------\n");
                }

                consola.Escribir("1. Menú Empleados\n");
                consola.Escribir("2. Iniciar Sesión Usuario (Login)\n");
                consola.Escribir("3. Menú Usuario (Solicitudes de Préstamo)\n");
                consola.Escribir("0. SALIR\n");

                string opcion = consola.LeerTexto();

                if (opcion == "0") break;

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            await MenuEmpleados.Menu(consola);
                            break;

                        case "2":
                            usuarioLogueado = await GestionLogin.LoginAsync(consola);
                            break;

                        case "3":
                            if (usuarioLogueado == null)
                            {
                                consola.Escribir(
                                    "\n[AVISO] Debe iniciar sesión (Opción 2) antes de acceder a la gestión de solicitudes.\n"
                                );

                                consola.Escribir("Presione cualquier tecla para continuar...\n");
                                consola.EsperarTecla();
                            }
                            else
                            {
                                await GestionSolicitudesPrestamos.Menu(
                                    usuarioLogueado.IdUsuario,
                                    consola
                                );
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
