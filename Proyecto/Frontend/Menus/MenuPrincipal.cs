using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ProyectoConjuntoMaterialPrestamo.Frontend;
using ProyectoConjuntoMaterialPrestamo.Models;

namespace ProyectoConjuntoMaterialPrestamo.Frontend.Menus
{
    internal class MenuPrincipal
    {

        public static Usuario? usuarioLogueado = null;

        static async public Task Menu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=================================================="); 
                Console.WriteLine(" SISTEMA DE BIBLIOTECA "); 
                Console.WriteLine("=================================================="); 
                if (usuarioLogueado != null)
                { 
                    Console.WriteLine($" [Sesión Usuario activa: {usuarioLogueado.NombreUsuario.Trim()} (ID: {usuarioLogueado.IdUsuario})]");
                    Console.WriteLine("--------------------------------------------------"); 
                }
                else
                {
                    Console.WriteLine(" [No hay sesión de usuario activa]");
                    Console.WriteLine("--------------------------------------------------");
                }

                Console.WriteLine("1. Menú Empleados");
                Console.WriteLine("2. Iniciar Sesión Usuario (Login)");
                Console.WriteLine("3. Menú Usuario (Solicitudes de Préstamo)");
                Console.WriteLine("0. SALIR");
                string opcion = LeerTexto("Opción: ");
                if (opcion == "0") break;
                try
                {
                    switch (opcion)
                    {
                        case "1": await MenuEmpleados.Menu(); break;
                        case "2": 
                                usuarioLogueado = await GestionLogin.LoginAsync(); break;
                        case "3":
                            if (usuarioLogueado == null) 
                            { 
                                Console.WriteLine("\\n[AVISO] Debe iniciar sesión (Opción 3) antes de acceder a la gestión de solicitudes.");
                                Console.WriteLine("Presione cualquier tecla para continuar...");
                                Console.ReadKey(); }
                            else
                            { 
                                await GestionUsuario.Menu(); 
                            }
                            break;
                         default:
                            Console.WriteLine("Opción no válida.");
                            Console.ReadKey();
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
