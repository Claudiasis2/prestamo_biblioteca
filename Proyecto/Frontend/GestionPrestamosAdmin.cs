using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    public static class GestionPrestamosAdmin
    {
        public static async Task Menu()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("==================================================");
                Console.WriteLine("      GESTIÓN DE PRÉSTAMOS Y DEVOLUCIONES         ");
                Console.WriteLine("==================================================");
                Console.WriteLine("1. Registrar entrega de material (Crear Préstamo)");
                Console.WriteLine("2. Registrar devolución de material");
                Console.WriteLine("3. Ver préstamos activos");
                Console.WriteLine("0. Volver al menú anterior");
                Console.WriteLine("--------------------------------------------------");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine()?.Trim() ?? string.Empty;

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            await RegistrarEntregaAsync();
                            break;
                        case "2":
                            await RegistrarDevolucionAsync();
                            break;
                        case "3":
                            await VerPrestamosActivosAsync();
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

        private static async Task RegistrarEntregaAsync()
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine("       REGISTRAR ENTREGA DE MATERIAL FISICO       ");
            Console.WriteLine("==================================================");

            
            var aprobadas = await PrestamoServicio.ListarSolicitudesAprobadasSinEntregarAsync();
            
            if (!aprobadas.Any())
            {
                Console.WriteLine("\n[INFORMACIÓN] No existen solicitudes aprobadas pendientes de entregar.");
                Console.WriteLine("Presione cualquier tecla para retornar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("SOLICITUDES APROBADAS EN ESPERA DE ENTREGA:");
            Console.WriteLine("----------------------------------------------------------------------------------");
            Console.WriteLine("ID Sol. | Fecha Solicitud   | Usuario                        | Material");
            Console.WriteLine("----------------------------------------------------------------------------------");
            foreach (var s in aprobadas)
            {
                string usuario = s.FkusuarioNavigation != null
                    ? $"{s.FkusuarioNavigation.NombreUsuario} {s.FkusuarioNavigation.PrimerApellido}".Trim()
                    : $"Usuario #{s.Fkusuario}";

                string titulo = s.FkmaterialBibliotecaNavigation?.Titulo ?? "Material Desconocido";
                Console.WriteLine($"{s.IdSolicitud,-7} | {s.FechaSolicitud:dd/MM/yyyy HH:mm} | {usuario,-30} | {titulo}");
            }
            Console.WriteLine("----------------------------------------------------------------------------------");

            Console.Write("\nIngrese el ID de la solicitud a entregar: ");
            if (!int.TryParse(Console.ReadLine(), out int idSolicitud) || idSolicitud <= 0)
            {
                Console.WriteLine("[ERROR] Debe ingresar un número entero positivo.");
                Console.ReadKey();
                return;
            }

            Console.Write("Ingrese la cantidad de días de préstamo (Presione ENTER para usar 7 días): ");
            string entradaDias = Console.ReadLine()?.Trim() ?? string.Empty;
            int diasPrestamo = 7;
            if (!string.IsNullOrEmpty(entradaDias) && int.TryParse(entradaDias, out int d) && d > 0)
            {
                diasPrestamo = d;
            }

            try
            {
                Console.WriteLine("\nAsignando ejemplar disponible y registrando préstamo...");
                int idPrestamo = await PrestamoServicio.CrearPrestamoAsync(idSolicitud, diasPrestamo);

                Console.WriteLine("\n==================================================");
                Console.WriteLine("        ¡PRÉSTAMO REGISTRADO CON ÉXITO!");
                Console.WriteLine("==================================================");
                Console.WriteLine($"Folio de préstamo generado: #{idPrestamo}");
                Console.WriteLine($"Plazo de devolución esperada: {diasPrestamo} días ({DateTime.Now.AddDays(diasPrestamo):dd/MM/yyyy})");
                Console.WriteLine("El ejemplar físico fue marcado como PRESTADO en estantería.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n[REGLA DE NEGOCIO] {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\n[DISPONIBILIDAD] {ex.Message}");
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("\n[ERROR BASE DE DATOS] No se pudo guardar la transacción de préstamo.");
            }
            catch (SqlException)
            {
                Console.WriteLine("\n[ERROR CONEXIÓN] Fallo de comunicación con SQL Server.");
            }
            
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static async Task RegistrarDevolucionAsync()
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine("            REGISTRAR DEVOLUCIÓN FÍSICA           ");
            Console.WriteLine("==================================================");

            var activos = await PrestamoServicio.ListarPrestamosActivosAsync();

            if (!activos.Any())
            {
                Console.WriteLine("\n[INFORMACIÓN] No hay préstamos activos en este momento.");
                Console.WriteLine("Presione cualquier tecla para retornar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("PRÉSTAMOS ACTIVOS EN CIRCULACIÓN:");
            Console.WriteLine("----------------------------------------------------------------------------------");
            Console.WriteLine("ID Préstamo | Ejemplar | Usuario                        | F. Límite   | Material");
            Console.WriteLine("----------------------------------------------------------------------------------");
            foreach (var p in activos)
            {
                string usuario = p.FkusuarioNavigation != null
                    ? $"{p.FkusuarioNavigation.NombreUsuario} {p.FkusuarioNavigation.PrimerApellido}".Trim()
                    : $"Usuario #{p.Fkusuario}";

                string titulo = p.FkejemplarNavigation?.FkmaterialBibliotecaNavigation?.Titulo ?? "Material Desconocido";
                int idEjemplar = p.FkejemplarNavigation?.IdEjemplar ?? 0;

                Console.WriteLine($"{p.IdPrestamo,-11} | #{idEjemplar,-7} | {usuario,-30} | {p.FechaLimiteDevolucion:dd/MM/yyyy} | {titulo}");
            }
            Console.WriteLine("----------------------------------------------------------------------------------");

            Console.Write("\nIngrese el ID del préstamo a DEVOLVER: ");
            if (!int.TryParse(Console.ReadLine(), out int idPrestamo) || idPrestamo <= 0)
            {
                Console.WriteLine("[ERROR] Debe ingresar un número entero positivo.");
                Console.ReadKey();
                return;
            }

            try
            {
                Console.WriteLine("\nProcesando devolución y liberando ejemplar...");
                await PrestamoServicio.RegistrarDevolucionAsync(idPrestamo);

                Console.WriteLine("\n==================================================");
                Console.WriteLine("       ¡DEVOLUCIÓN REGISTRADA CON ÉXITO!");
                Console.WriteLine("==================================================");
                Console.WriteLine($"Préstamo #{idPrestamo} marcado como DEVUELTO / FINALIZADO.");
                Console.WriteLine("El ejemplar físico ha retornado al estado DISPONIBLE en estantería.");
                Console.WriteLine("El cupo del usuario se ha liberado.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n[REGLA DE NEGOCIO] {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\n[ESTADO] {ex.Message}");
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("\n[ERROR BASE DE DATOS] No se pudo guardar la devolución.");
            }
            catch (SqlException)
            {
                Console.WriteLine("\n[ERROR CONEXIÓN] Inconveniente de comunicación con SQL Server.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static async Task VerPrestamosActivosAsync()
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine("             LISTADO DE PRÉSTAMOS ACTIVOS         ");
            Console.WriteLine("==================================================");

            var activos = await PrestamoServicio.ListarPrestamosActivosAsync();

            if (!activos.Any())
            {
                Console.WriteLine("\n[INFORMACIÓN] No hay préstamos en circulación actualmente.");
            }
            else
            {
                Console.WriteLine("ID Préstamo | Ejemplar | Usuario                        | F. Préstamo | F. Límite   | Material");
                Console.WriteLine("-------------------------------------------------------------------------------------------------");
                foreach (var p in activos)
                {
                    string usuario = p.FkusuarioNavigation != null
                        ? $"{p.FkusuarioNavigation.NombreUsuario} {p.FkusuarioNavigation.PrimerApellido}".Trim()
                        : $"Usuario #{p.Fkusuario}";

                    string titulo = p.FkejemplarNavigation?.FkmaterialBibliotecaNavigation?.Titulo ?? "Material Desconocido";
                    int idEjemplar = p.FkejemplarNavigation?.IdEjemplar ?? 0;

                    Console.WriteLine($"{p.IdPrestamo,-11} | #{idEjemplar,-7} | {usuario,-30} | {p.FechaSalida:dd/MM/yyyy} | {p.FechaLimiteDevolucion:dd/MM/yyyy} | {titulo}");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para retornar al menú...");
            Console.ReadKey();
        }
    }
}