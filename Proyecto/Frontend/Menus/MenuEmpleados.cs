using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Frontend.Menus
{
    internal class MenuEmpleados
    {
        static async public Task Menu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==================================================");
                Console.WriteLine(" MENU DE EMPLEADOS ");
                Console.WriteLine("==================================================");
                if (MenuPrincipal.usuarioLogueado != null)
                {
                    Console.WriteLine($" [Sesión Usuario activa: {MenuPrincipal.usuarioLogueado.NombreUsuario.Trim()} (ID: {MenuPrincipal.usuarioLogueado.IdUsuario})]");
                    Console.WriteLine("--------------------------------------------------");
                }
                Console.WriteLine("1. Menu Administrador");
                Console.WriteLine("=====================================================");
                Console.WriteLine("8. Gestión de recepción de préstamo de materiales");
                Console.WriteLine("9. Gestión de solicitudes de préstamo de materiales");
                Console.WriteLine("0. SALIR");
                string opcion = LeerTexto("Opción: ");
                if (opcion == "0") break;
                try
                {
                    switch (opcion)
                    {
                        case "1": await MenuAdministrador.Menu(); break;
                        case "2":
                            bool convertido = int.TryParse(LeerTexto("Id del usuario: "), out int idUsuario);
                            if (!convertido)
                            {
                                Console.WriteLine("Id de usuario no válido.");
                                continue;
                            }
                            await GestionSolicitudesPrestamos.Menu(idUsuario); break;

                        default:
                            Console.WriteLine("Opción no válida.");
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("La operación SQL ha fallado.");
                    Console.WriteLine(ex.Message);
                }
                catch (DbUpdateException)
                {
                    Console.WriteLine(
                    "No se pudieron guardar los cambios.");
                }
            }
        }

        static string LeerTexto(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine()?.Trim() ?? "";
        }
    }
}
