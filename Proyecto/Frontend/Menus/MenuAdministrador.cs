
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ProyectoConjuntoMaterialPrestamo.Frontend.Menus
{
    internal class MenuAdministrador
    {
        public static async Task Menu(IConsola consola)
        {
            while (true)
            {
                consola.Limpiar();
                consola.Escribir("==================================================\n");
                consola.Escribir(" MENU DE ADMINISTRADOR ");
                consola.Escribir("==================================================\n");
                consola.Escribir("1. Gestión de Usuarios\n");
                consola.Escribir("2. Gestión de empleados\n");
                consola.Escribir("3. Gestión de estados de ejemplar\n");
                consola.Escribir("4. Gestión de estados de préstamo\n");
                consola.Escribir("5. Gestión de estados de solicitud de préstamo\n");
                consola.Escribir("6. Gestión de solicitudes de préstamo\n");
                consola.Escribir("7. Gestión de préstamos\n");
                consola.Escribir("8. Gestión de materiales\n");
                consola.Escribir("=====================================================\n");
                consola.Escribir("9. Gestión de recepción de préstamo de materiales\n");
                consola.Escribir("10. Gestión de solicitudes de préstamo de materiales\n");
                consola.Escribir("0. SALIR\n");

                string opcion = consola.LeerTexto();

                if (opcion == "0") break;

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            await GestionUsuario.Menu(consola);
                            break;

                        case "2":
                            await GestionEmpleados.Menu(consola);
                            break;

                        case "3":
                            await GestionEstadoEjemplares.Menu(consola);
                            break;

                        case "4":
                            await GestionEstadoPrestamos.Menu(consola);
                            break;

                        case "5":
                            await GestionEstadoSolicitudPrestamos.Menu(consola);
                            break;

                        case "6":
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

                        case "7":
                            await GestionPrestamos.Menu(consola);
                            break;

                        case "8":
                            await GestionMaterial.Menu(consola);
                            break;

                        case "9":
                            await GestionPrestamosAdmin.Menu(consola);
                            break;

                        case "10":
                            await GestionSolicitudesAdmin.Menu(consola);
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
