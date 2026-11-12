using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ProyectoConjuntoMaterialPrestamo.Frontend;

namespace ProyectoConjuntoMaterialPrestamo.Frontend.Menus
{
    internal class MenuPrincipal
    {
        static async public Task Menu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Menú Administrador");
                Console.WriteLine("2. Perfil Usuario");
                Console.WriteLine("3. Loggin");
                Console.WriteLine("4. Menú Usuario (Solicitudes)");
                Console.WriteLine("0. SALIR");
                string opcion = LeerTexto("Opción: ");
                if (opcion == "0") break;
                try
                {
                    switch (opcion)
                    {
                        case "1": await MenuAdministrador.Menu(); break;
                        case "2": await GestionUsuario.Menu(); break;
                        case "3": GestionLogin.Login(); break;
                        case "4": await GestionSolicitudesPrestamos.Menu(24); break;
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
