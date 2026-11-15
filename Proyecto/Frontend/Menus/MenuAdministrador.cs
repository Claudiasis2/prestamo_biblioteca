using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Frontend.Menus
{
    internal class MenuAdministrador
    {
        static async public Task Menu()
        {
            while (true)
            {
                Console.Clear();
                //Console.WriteLine("1. GESTION DE MATERIALES");
                //Console.WriteLine("2. GESTION DE USUARIOS");
                //Console.WriteLine("3. GESTION DE CD");
                //Console.WriteLine("4. GESTION DE DVD");
                //Console.WriteLine("5. GESTION DE LIBROS");
                //Console.WriteLine("6. GESTION DE EMPLEADOS");
                //Console.WriteLine("7. GESTION DE ESTADOS (PROXIMAMENTE)");
                //Console.WriteLine("8. GESTION DE SOLICITUDES DE PRESTAMOS");
                //Console.WriteLine("9. RECEPCION DE MATERIALES");

                Console.WriteLine("1. Gestión de Usuarios");
                Console.WriteLine("2. Gestión de empleados");
                Console.WriteLine("3. Gestión de estados de ejemplar");
                Console.WriteLine("4. Gestión de estados de préstamo");
                Console.WriteLine("5. Gestión de estados de solicitud de préstamo");
                Console.WriteLine("6. Gestión de solicitudes de préstamo");
                Console.WriteLine("7. Gestión de préstamos");
                Console.WriteLine("8. Gestión de materiales");
                Console.WriteLine("=====================================================");
                Console.WriteLine("9. Gestión de recepción de préstamo de materiales");
                Console.WriteLine("10. Gestión de solicitudes de préstamo de materiales");
                Console.WriteLine("0. SALIR");
                string opcion = LeerTexto("Opción: ");
                if (opcion == "0") break;
                try
                {
                    switch (opcion)
                    {
                        case "1": await GestionUsuario.Menu(); break;
                        case "2": await GestionEmpleados.Menu(); break;
                        case "3": await GestionEstadoEjemplares.Menu(); break;
                        case "4": await GestionEstadoPrestamos.Menu(); break;
                        case "5": await GestionEstadoSolicitudPrestamos.Menu(); break;
                        case "6":
                            bool convertido = int.TryParse(LeerTexto("Id del usuario: "), out int idUsuario);
                            if (!convertido)
                            {
                                Console.WriteLine("Id de usuario no válido.");
                                continue;
                            }
                            await GestionSolicitudesPrestamos.Menu(idUsuario); break;
                        case "7": await GestionPrestamos.Menu(); break;
                        case "8": await GestionMaterial.Menu(); break;
                        case "9": await GestionPrestamosAdmin.Menu(); break;
                        case "10": await GestionSolicitudesAdmin.Menu(); break;

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
