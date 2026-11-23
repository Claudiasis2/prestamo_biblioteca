
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ProyectoConjuntoMaterialPrestamo.Frontend.Menus
{
    internal class MenuEmpleados
    {
        public static async Task Menu(IConsola consola)
        {
            while (true)
            {
                consola.Limpiar();

                consola.Escribir("==================================================\n");
                consola.Escribir(" MENU DE EMPLEADOS \n");
                consola.Escribir("==================================================\n");

                if (MenuPrincipal.usuarioLogueado != null)
                {
                    consola.Escribir(
                        $" [Sesión Usuario activa: {MenuPrincipal.usuarioLogueado.NombreUsuario.Trim()} (ID: {MenuPrincipal.usuarioLogueado.IdUsuario})]\n"
                    );

                    consola.Escribir("--------------------------------------------------\n");
                }

                consola.Escribir("1. Menu Administrador\n");
                consola.Escribir("=====================================================\n");
                consola.Escribir("8. Gestión de recepción de préstamo de materiales\n");
                consola.Escribir("9. Gestión de solicitudes de préstamo de materiales\n");
                consola.Escribir("0. SALIR\n");

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
                            bool convertido = int.TryParse(
                                consola.LeerTexto(),
                                out int idUsuario
                            );

                            if (!convertido)
                            {
                                consola.Escribir("Id de usuario no válido.\n");
                                continue;
                            }

                            await GestionSolicitudesPrestamos.Menu(idUsuario, consola);
                            break;

                        default:
                            consola.Escribir("Opción no válida.\n");
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
