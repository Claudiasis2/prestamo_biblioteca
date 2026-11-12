using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    public static class GestionSolicitudesAdmin
    {
        public static async Task Menu()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("==================================================");
                Console.WriteLine("       REVISIÓN Y GESTIÓN DE SOLICITUDES          ");
                Console.WriteLine("==================================================");

                try
                {
                    var pendientes = await SolicitudPrestamoServicio.ListarPendientesAsync();

                    if (!pendientes.Any())
                    {
                        Console.WriteLine("\n[INFORMACIÓN] No hay solicitudes pendientes de revisión.");
                    }
                    else
                    {
                        Console.WriteLine($"Solicitudes pendientes de atención: {pendientes.Count}\n");
                        Console.WriteLine("ID Sol. | Fecha               | Usuario                        | Material");
                        Console.WriteLine("----------------------------------------------------------------------------------");
                        foreach (var s in pendientes)
                        {
                            string usuario = s.FkusuarioNavigation != null ? $"{s.FkusuarioNavigation.NombreUsuario} {s.FkusuarioNavigation.PrimerApellido}".Trim() : $"Usuario #{s.Fkusuario}";
                            string titulo = s.FkmaterialBibliotecaNavigation?.Titulo ?? "Material Desconocido";
                            Console.WriteLine($"{s.IdSolicitud,-7} | {s.FechaSolicitud:dd/MM/yyyy HH:mm} | {usuario,-30} | {titulo}");
                        }
                    }

                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("1. Aprobar solicitud");
                    Console.WriteLine("2. Rechazar solicitud");
                    Console.WriteLine("0. Volver al menú anterior");
                    Console.WriteLine("--------------------------------------------------");
                    Console.Write("Seleccione una opción: ");

                    string opcion = Console.ReadLine()?.Trim() ?? string.Empty;

                    switch (opcion)
                    {
                        case "1":
                            await ProcesarAprobacionAsync();
                            break;
                        case "2":
                            await ProcesarRechazoAsync();
                            break;
                        case "0":
                            salir = true;
                            break;
                        default:
                            Console.WriteLine("\nOpción no válida. Presione cualquier tecla para continuar...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nOcurrió un inconveniente: {ex.Message}");
                    Console.WriteLine("Presione cualquier tecla para reintentar...");
                    Console.ReadKey();
                }
            }
        }

        private static async Task ProcesarAprobacionAsync()
        {
            Console.Write("\nIngrese el ID de la solicitud a APROBAR: ");
            if (!int.TryParse(Console.ReadLine(), out int idSolicitud) || idSolicitud <= 0)
            {
                Console.WriteLine("[ERROR] Debe ingresar un número entero positivo.");
                Console.ReadKey();
                return;
            }

            try
            {
                //await SolicitudPrestamoServicio.AprobarSolicitudAsync(idSolicitud);
                Console.WriteLine($"\n[ÉXITO] La solicitud #{idSolicitud} ha sido APROBADA correctamente.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n[REGLA DE NEGOCIO] {ex.Message}");
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("\n[ERROR BASE DE DATOS] No se pudo actualizar el estado de la solicitud.");
            }
            catch (SqlException)
            {
                Console.WriteLine("\n[ERROR DE CONEXIÓN] Fallo de comunicación con SQL Server.");
            }

            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static async Task ProcesarRechazoAsync()
        {
            Console.Write("\nIngrese el ID de la solicitud a RECHAZAR: ");
            if (!int.TryParse(Console.ReadLine(), out int idSolicitud) || idSolicitud <= 0)
            {
                Console.WriteLine("[ERROR] Debe ingresar un número entero positivo.");
                Console.ReadKey();
                return;
            }

            try
            {
                //await SolicitudPrestamoServicio.RechazarSolicitudAsync(idSolicitud);
                Console.WriteLine($"\n[ÉXITO] La solicitud #{idSolicitud} ha sido RECHAZADA. El cupo del usuario se ha liberado.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n[REGLA DE NEGOCIO] {ex.Message}");
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("\n[ERROR BASE DE DATOS] No se pudo actualizar el estado de la solicitud.");
            }
            catch (SqlException)
            {
                Console.WriteLine("\n[ERROR DE CONEXIÓN] Fallo de comunicación con SQL Server.");
            }

            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}