using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    public static class GestionSolicitudesPrestamos
    {

        public static async Task Menu(int idUsuario)
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("==================================================");
                Console.WriteLine("          GESTIÓN DE SOLICITUDES DE PRÉSTAMO       ");
                Console.WriteLine("==================================================");

                try
                {
                    int cupoDisponible = await SolicitudPrestamoServicio.ObtenerCupoDisponibleAsync(idUsuario);
                    Console.WriteLine($"Cupo de materiales disponible: {cupoDisponible} de 3");
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("1. Solicitar nuevo préstamo (1, 2 o 3 materiales)");
                    Console.WriteLine("2. Ver mi historial de solicitudes");
                    Console.WriteLine("0. Volver al menú anterior");
                    Console.WriteLine("--------------------------------------------------");
                    Console.Write("Seleccione una opción: ");

                    string opcion = Console.ReadLine()?.Trim() ?? string.Empty;

                    switch (opcion)
                    {
                        case "1":
                            await RealizarSolicitudAsync(idUsuario);
                            break;
                        case "2":
                            await VerMisSolicitudesAsync(idUsuario);
                            break;
                        case "0":
                            salir = true;
                            break;
                        default:
                            Console.WriteLine("\n[ERROR] Opción no válida. Presione cualquier tecla para continuar...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[ERROR] Ocurrió un inconveniente: {ex.Message}");
                    Console.WriteLine("Presione cualquier tecla para reintentar...");
                    Console.ReadKey();
                }
            }
        }

        private static async Task RealizarSolicitudAsync(int idUsuario)
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine("            NUEVA SOLICITUD DE PRÉSTAMO           ");
            Console.WriteLine("==================================================");

            int cupoDisponible = await SolicitudPrestamoServicio.ObtenerCupoDisponibleAsync(idUsuario);

            if (cupoDisponible <= 0)
            {
                Console.WriteLine("\n[AVISO] Ha alcanzado el límite máximo de 3 materiales pendientes.");
                Console.WriteLine("Debe devolver sus materiales prestados o esperar la resolución de sus solicitudes.");
                Console.WriteLine("\nPresione cualquier tecla para retornar al menú...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Tiene {cupoDisponible} cupo(s) disponible(s).\n");

            var materiales = await MaterialBibliotecaServicio.ListarDisponiblesConTipoAsync();
            if (!materiales.Any())
            {
                Console.WriteLine("[AVISO] No hay materiales registrados en la biblioteca.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("CATÁLOGO DE MATERIALES DISPONIBLES:");
            Console.WriteLine("--------------------------------------------------");
            foreach (var mat in materiales)
            {
                Console.WriteLine($" {mat.IdMaterial,-4} | {mat.Titulo,-30} | ({mat.Tipo})");
            }
            Console.WriteLine("--------------------------------------------------");

            Console.WriteLine($"\n¿Cuántos materiales desea solicitar? (Máximo {cupoDisponible}): ");
            if (!int.TryParse(Console.ReadLine(), out int cantidad) || cantidad <= 0 || cantidad > cupoDisponible)
            {
                Console.WriteLine($"\n[ERROR] Debe ingresar un número entre 1 y {cupoDisponible}.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var idsSeleccionados = new List<int>();
            for (int i = 1; i <= cantidad; i++)
            {
                Console.Write($"Ingrese el ID del material #{i}: ");
                if (int.TryParse(Console.ReadLine(), out int idMat) && idMat > 0)
                {
                    if (idsSeleccionados.Contains(idMat))
                    {
                        Console.WriteLine(" [AVISO] Ya seleccionó este material en esta lista. Intente con otro.");
                        i--;
                    }
                    else
                    {
                        idsSeleccionados.Add(idMat);
                    }
                }
                else
                {
                    Console.WriteLine(" [ERROR] ID de material inválido.");
                    i--;
                }
            }

            try
            {
                Console.WriteLine("\nProcesando solicitud de préstamo...");
                var idsSolicitudesGeneradas = await SolicitudPrestamoServicio.CrearSolicitudAsync(idUsuario, idsSeleccionados);

                Console.WriteLine("\n==================================================");
                Console.WriteLine("     ¡SOLICITUD REGISTRADA CON ÉXITO!");
                Console.WriteLine("==================================================");
                Console.WriteLine($"Folios de solicitud generados: {string.Join(", ", idsSolicitudesGeneradas)}");
                Console.WriteLine("Estado inicial: PENDIENTE (esperando revisión por un empleado).");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n[REGLA DE NEGOCIO] {ex.Message}");
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("\n[ERROR DE BASE DE DATOS] No se pudo guardar la solicitud debido a una restricción de la base de datos.");
            }
            catch (SqlException)
            {
                Console.WriteLine("\n[ERROR DE CONEXIÓN] Inconveniente de comunicación con SQL Server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR INESPERADO] {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static async Task VerMisSolicitudesAsync(int idUsuario)
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine("            MIS SOLICITUDES DE PRÉSTAMO           ");
            Console.WriteLine("==================================================");

            var solicitudes = await SolicitudPrestamoServicio.ListarPorUsuarioAsync(idUsuario);

            if (!solicitudes.Any())
            {
                Console.WriteLine("\nNo registra solicitudes en el sistema.");
            }
            else
            {
                Console.WriteLine("ID Solicitud | Fecha               | Material                          | Estado");
                Console.WriteLine("----------------------------------------------------------------------------------");
                foreach (var s in solicitudes)
                {
                    string titulo = s.FkmaterialBibliotecaNavigation?.Titulo ?? "Material Desconocido";
                    //string estado = s.FkestadoNavigation?.NombreEstado ?? "N/A";
                    string estado = "N/A";
                    Console.WriteLine($"{s.IdSolicitud,-12} | {s.FechaSolicitud:dd/MM/yyyy HH:mm} | {titulo,-33} | {estado}");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para retornar al menú...");
            Console.ReadKey();
        }
    }
}
