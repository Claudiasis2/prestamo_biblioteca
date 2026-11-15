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
        static async public Task Menu()
        {
            Usuario? usuarioLogueado = null;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=================================================="); 
                Console.WriteLine(" SISTEMA DE BIBLIOTECA "); 
                Console.WriteLine("=================================================="); 
                  if (usuarioLogueado != null)
                  { 
                    Console.WriteLine($" [Sesión activa: {usuarioLogueado.NombreUsuario.Trim()} (ID: {usuarioLogueado.IdUsuario})]");
                    Console.WriteLine("--------------------------------------------------"); 
                }

                Console.WriteLine("1. Menú Administrador");
                Console.WriteLine("2. Perfil Usuario");
                Console.WriteLine("3. Iniciar Sesión(Login)");
                Console.WriteLine("4. Menú Usuario (Solicitudes de Préstamo)");
                Console.WriteLine("0. SALIR");
                string opcion = LeerTexto("Opción: ");
                if (opcion == "0") break;
                try
                {
                    switch (opcion)
                    {
                        case "1": await MenuAdministrador.Menu(); break;
                        case "2": await GestionUsuario.Menu(); break;
                        case "3": 
                                usuarioLogueado = await GestionLogin.LoginAsync(); break;
                        case "4":
                            if (usuarioLogueado == null) 
                            { 
                                Console.WriteLine("\\n[AVISO] Debe iniciar sesión (Opción 3) antes de acceder a la gestión de solicitudes.");
                                Console.WriteLine("Presione cualquier tecla para continuar...");
                                Console.ReadKey(); }
                            else
                            { 
                                await GestionSolicitudesPrestamos.Menu(usuarioLogueado.IdUsuario); 
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
