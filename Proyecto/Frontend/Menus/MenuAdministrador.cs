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
                Console.WriteLine("1. GESTION DE MATERIALES");
                Console.WriteLine("2. GESTION DE USUARIOS");
                Console.WriteLine("3. GESTION DE CD");
                Console.WriteLine("4. GESTION DE DVD");
                Console.WriteLine("5. GESTION DE LIBROS");
                Console.WriteLine("6. GESTION DE EMPLEADOS");
                Console.WriteLine("7. GESTION DE ESTADOS (PROXIMAMENTE)");
                Console.WriteLine("8. GESTION DE SOLICITUDES DE PRESTAMOS");
                Console.WriteLine("9. RECEPCION DE MATERIALES");
                Console.WriteLine("0. SALIR");
                string opcion = LeerTexto("Opción: ");
                if (opcion == "0") break;
                try
                {
                    switch (opcion)
                    {
                        case "1": await GestionMaterial.Menu(); break;
                        case "2": await GestionUsuario.Menu(); break;
                        case "3": await GestionCD.Menu(); break;
                        case "4": await GestionDVD.Menu(); break;
                        case "5": await GestionLibros.Menu(); break;
                        case "6": await GestionEmpleados.Menu(); break;
                        case "7": await GestionEstados.Menu(); break;
                        case "8": await GestionSolicitudesAdmin.Menu(); break;
                        case "9": await GestionPrestamosAdmin.Menu(); break;
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
