
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoConjuntoMaterialPrestamo.Servicios;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    public static class GestionSolicitudesPrestamos
    {
        public static async Task Menu(int idUsuario, IConsola consola)
        {
            bool salir = false;

            while (!salir)
            {
                consola.Limpiar();
                consola.Escribir("==================================================\n");
                consola.Escribir("          GESTIÓN DE SOLICITUDES DE PRÉSTAMO       \n");
                consola.Escribir("==================================================\n");

                try
                {
                    int cupoDisponible =
                        await SolicitudPrestamoServicio.ObtenerCupoDisponibleAsync(idUsuario);

                    consola.Escribir($"Cupo de materiales disponible: {cupoDisponible} de 3\n");
                    consola.Escribir("--------------------------------------------------\n");
                    consola.Escribir("1. Solicitar nuevo préstamo (1, 2 o 3 materiales)\n");
                    consola.Escribir("2. Ver mi historial de solicitudes\n");
                    consola.Escribir("3. Cancelar una solicitud pendiente\n");
                    consola.Escribir("0. Volver al menú anterior\n");
                    consola.Escribir("--------------------------------------------------\n");
                    consola.Escribir("Seleccione una opción: ");

                    string opcion = consola.LeerTexto();

                    switch (opcion)
                    {
                        case "1":
                            await RealizarSolicitudAsync(idUsuario, consola);
                            break;

                        case "2":
                            await VerMisSolicitudesAsync(idUsuario, consola);
                            break;

                        case "3":
                            await CancelarSolicitudAsync(idUsuario, consola);
                            break;

                        case "0":
                            salir = true;
                            break;

                        default:
                            consola.Escribir("\n[ERROR] Opción no válida. Presione cualquier tecla para continuar...\n");
                            consola.EsperarTecla();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    consola.Escribir($"\n[ERROR] Ocurrió un inconveniente: {ex.Message}\n");
                    consola.Escribir("Presione cualquier tecla para reintentar...\n");
                    consola.EsperarTecla();
                }
            }
        }

        private static async Task RealizarSolicitudAsync(int idUsuario, IConsola consola)
        {
            consola.Limpiar();
            consola.Escribir("==================================================\n");
            consola.Escribir("            NUEVA SOLICITUD DE PRÉSTAMO           \n");
            consola.Escribir("==================================================\n");

            int cupoDisponible =
                await SolicitudPrestamoServicio.ObtenerCupoDisponibleAsync(idUsuario);

            if (cupoDisponible <= 0)
            {
                consola.Escribir("\n[AVISO] Ha alcanzado el límite máximo de 3 materiales pendientes.\n");
                consola.Escribir("Debe devolver sus materiales prestados o esperar la resolución de sus solicitudes.\n");
                consola.Escribir("\nPresione cualquier tecla para retornar al menú...\n");
                consola.EsperarTecla();
                return;
            }

            consola.Escribir($"Tiene {cupoDisponible} cupo(s) disponible(s).\n\n");

            var materiales =
                await MaterialBibliotecaServicio.ListarDisponiblesConTipoAsync();

            if (!materiales.Any())
            {
                consola.Escribir("[AVISO] No hay materiales registrados en la biblioteca.\n");
                consola.EsperarTecla();
                return;
            }

            consola.Escribir("CATÁLOGO DE MATERIALES DISPONIBLES:\n");
            consola.Escribir("--------------------------------------------------\n");

            foreach (var mat in materiales)
            {
                consola.Escribir($" {mat.IdMaterial,-4} | {mat.Titulo,-30} | ({mat.Tipo})\n");
            }

            consola.Escribir("--------------------------------------------------\n");
            consola.Escribir($"\n¿Cuántos materiales desea solicitar? (Máximo {cupoDisponible}): ");

            if (!int.TryParse(consola.LeerTexto(), out int cantidad) ||
                cantidad <= 0 || cantidad > cupoDisponible)
            {
                consola.Escribir($"\n[ERROR] Debe ingresar un número entre 1 y {cupoDisponible}.\n");
                consola.Escribir("Presione cualquier tecla para continuar...\n");
                consola.EsperarTecla();
                return;
            }

            var idsSeleccionados = new List<int>();

            for (int i = 1; i <= cantidad; i++)
            {
                consola.Escribir($"Ingrese el ID del material #{i}: ");

                if (int.TryParse(consola.LeerTexto(), out int idMat) && idMat > 0)
                {
                    if (idsSeleccionados.Contains(idMat))
                    {
                        consola.Escribir(" [AVISO] Ya seleccionó este material en esta lista. Intente con otro.\n");
                        i--;
                    }
                    else
                    {
                        idsSeleccionados.Add(idMat);
                    }
                }
                else
                {
                    consola.Escribir(" [ERROR] ID de material inválido.\n");
                    i--;
                }
            }

            try
            {
                consola.Escribir("\nProcesando solicitud de préstamo...\n");

                var idsSolicitudesGeneradas =
                    await SolicitudPrestamoServicio.CrearSolicitudAsync(
                        idUsuario, idsSeleccionados);

                consola.Escribir("\n==================================================\n");
                consola.Escribir("     ¡SOLICITUD REGISTRADA CON ÉXITO!\n");
                consola.Escribir("==================================================\n");
                consola.Escribir($"Folios de solicitud generados: {string.Join(", ", idsSolicitudesGeneradas)}\n");
                consola.Escribir("Estado inicial: PENDIENTE (esperando revisión por un empleado).\n");
            }
            catch (ArgumentException ex)
            {
                consola.Escribir($"\n[REGLA DE NEGOCIO] {ex.Message}\n");
            }
            catch (DbUpdateException)
            {
                consola.Escribir("\n[ERROR DE BASE DE DATOS] No se pudo guardar la solicitud debido a una restricción de la base de datos.\n");
            }
            catch (SqlException)
            {
                consola.Escribir("\n[ERROR DE CONEXIÓN] Inconveniente de comunicación con SQL Server.\n");
            }
            catch (Exception ex)
            {
                consola.Escribir($"\n[ERROR INESPERADO] {ex.Message}\n");
            }

            consola.Escribir("\nPresione cualquier tecla para continuar...\n");
            consola.EsperarTecla();
        }

        private static async Task VerMisSolicitudesAsync(int idUsuario, IConsola consola)
        {
            consola.Limpiar();
            consola.Escribir("==================================================\n");
            consola.Escribir("            MIS SOLICITUDES DE PRÉSTAMO           \n");
            consola.Escribir("==================================================\n");

            var solicitudes =
                await SolicitudPrestamoServicio.ListarPorUsuarioAsync(idUsuario);

            if (!solicitudes.Any())
            {
                consola.Escribir("\nNo registra solicitudes en el sistema.\n");
            }
            else
            {
                consola.Escribir("ID Solicitud | Fecha               | Material                          | Estado\n");
                consola.Escribir("----------------------------------------------------------------------------------\n");

                foreach (var s in solicitudes)
                {
                    string titulo = s.FkmaterialBibliotecaNavigation?.Titulo ?? "Material Desconocido";
                    string estado = s.FkestadoSolicitudPrestamoNavigation?.NombreEstado ?? "N/A";

                    consola.Escribir(
                        $"{s.IdSolicitud,-12} | {s.FechaSolicitud:dd/MM/yyyy HH:mm} | {titulo,-33} | {estado}\n");
                }
            }

            consola.Escribir("\nPresione cualquier tecla para retornar al menú...\n");
            consola.EsperarTecla();
        }

        private static async Task CancelarSolicitudAsync(int idUsuario, IConsola consola)
        {
            consola.Limpiar();
            consola.Escribir("==================================================\n");
            consola.Escribir(" CANCELAR SOLICITUD PENDIENTE \n");
            consola.Escribir("==================================================\n");

            var solicitudes =
                await SolicitudPrestamoServicio.ListarPorUsuarioAsync(idUsuario);

            var pendientes =
                solicitudes.Where(s => s.FkestadoSolicitudPrestamo == 1).ToList();

            if (!pendientes.Any())
            {
                consola.Escribir("\nNo tiene solicitudes en estado Pendiente para cancelar.\n");
                consola.Escribir("Presione cualquier tecla para retornar...\n");
                consola.EsperarTecla();
                return;
            }

            consola.Escribir("SUS SOLICITUDES PENDIENTES:\n");
            consola.Escribir("----------------------------------------------------------------------------------\n");
            consola.Escribir("ID Solicitud | Fecha | Material\n");
            consola.Escribir("----------------------------------------------------------------------------------\n");

            foreach (var s in pendientes)
            {
                string titulo =
                    s.FkmaterialBibliotecaNavigation?.Titulo ?? "Material Desconocido";

                consola.Escribir(
                    $"{s.IdSolicitud,-12} | {s.FechaSolicitud:dd/MM/yyyy HH:mm} | {titulo}\n");
            }

            consola.Escribir("----------------------------------------------------------------------------------\n");
            consola.Escribir("\nIngrese el ID de la solicitud a cancelar: ");

            if (!int.TryParse(consola.LeerTexto(), out int idSolicitud) || idSolicitud <= 0)
            {
                consola.Escribir("[ERROR] Debe ingresar un número entero positivo.\n");
                consola.EsperarTecla();
                return;
            }

            try
            {
                await SolicitudPrestamoServicio.CancelarSolicitudUsuarioAsync(
                    idSolicitud, idUsuario);

                consola.Escribir(
                    $"\n[ÉXITO] La solicitud #{idSolicitud} ha sido CANCELADA y su cupo se ha liberado.\n");
            }
            catch (ArgumentException ex)
            {
                consola.Escribir($"\n[AVISO] {ex.Message}\n");
            }
            catch (InvalidOperationException ex)
            {
                consola.Escribir($"\n[ESTADO] {ex.Message}\n");
            }
            catch (DbUpdateException)
            {
                consola.Escribir("\n[ERROR DE BASE DE DATOS] No se pudo cancelar la solicitud.\n");
            }

            consola.Escribir("\nPresione cualquier tecla para continuar...\n");
            consola.EsperarTecla();
        }
    }
}
