
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Servicios;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    public static class GestionSolicitudesAdmin
    {
        public static async Task Menu(IConsola consola)
        {
            bool salir = false;

            while (!salir)
            {
                consola.Limpiar();
                consola.Escribir("==================================================\n");
                consola.Escribir("       REVISIÓN Y GESTIÓN DE SOLICITUDES          \n");
                consola.Escribir("==================================================\n");

                try
                {
                    var pendientes = await SolicitudPrestamoServicio.ListarPendientesAsync();

                    if (!pendientes.Any())
                    {
                        consola.Escribir("\n[INFORMACIÓN] No hay solicitudes pendientes de revisión.\n");
                    }
                    else
                    {
                        consola.Escribir($"Solicitudes pendientes de atención: {pendientes.Count}\n\n");
                        consola.Escribir("ID Sol. | Fecha               | Usuario                        | Material\n");
                        consola.Escribir("----------------------------------------------------------------------------------\n");

                        foreach (var s in pendientes)
                        {
                            string usuario = s.FkusuarioNavigation != null
                                ? $"{s.FkusuarioNavigation.NombreUsuario} {s.FkusuarioNavigation.PrimerApellido}".Trim()
                                : $"Usuario #{s.Fkusuario}";

                            string titulo = s.FkmaterialBibliotecaNavigation?.Titulo ?? "Material Desconocido";

                            consola.Escribir($"{s.IdSolicitud,-7} | {s.FechaSolicitud:dd/MM/yyyy HH:mm} | {usuario,-30} | {titulo}\n");
                        }
                    }

                    consola.Escribir("--------------------------------------------------\n");
                    consola.Escribir("1. Aprobar solicitud\n");
                    consola.Escribir("2. Rechazar solicitud\n");
                    consola.Escribir("0. Volver al menú anterior\n");
                    consola.Escribir("--------------------------------------------------\n");
                    consola.Escribir("Seleccione una opción: ");

                    string opcion = consola.LeerTexto();

                    switch (opcion)
                    {
                        case "1":
                            await ProcesarAprobacionAsync(consola);
                            break;
                        case "2":
                            await ProcesarRechazoAsync(consola);
                            break;
                        case "0":
                            salir = true;
                            break;
                        default:
                            consola.Escribir("\nOpción no válida. Presione cualquier tecla para continuar...\n");
                            consola.EsperarTecla();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    consola.Escribir($"\nOcurrió un inconveniente: {ex.Message}\n");
                    consola.Escribir("Presione cualquier tecla para reintentar...\n");
                    consola.EsperarTecla();
                }
            }
        }

        private static async Task ProcesarAprobacionAsync(IConsola consola)
        {
            consola.Escribir("\nIngrese el ID de la solicitud a APROBAR: ");

            if (!int.TryParse(consola.LeerTexto(), out int idSolicitud) || idSolicitud <= 0)
            {
                consola.Escribir("[ERROR] Debe ingresar un número entero positivo.\n");
                consola.EsperarTecla();
                return;
            }

            try
            {
                await SolicitudPrestamoServicio.AprobarSolicitudAsync(idSolicitud);

                consola.Escribir($"\n[ÉXITO] La solicitud #{idSolicitud} ha sido APROBADA correctamente.\n");
            }
            catch (ArgumentException ex)
            {
                consola.Escribir($"\n[REGLA DE NEGOCIO] {ex.Message}\n");
            }
            catch (DbUpdateException)
            {
                consola.Escribir("\n[ERROR BASE DE DATOS] No se pudo actualizar el estado de la solicitud.\n");
            }
            catch (SqlException)
            {
                consola.Escribir("\n[ERROR DE CONEXIÓN] Fallo de comunicación con SQL Server.\n");
            }

            consola.Escribir("Presione cualquier tecla para continuar...\n");
            consola.EsperarTecla();
        }

        private static async Task ProcesarRechazoAsync(IConsola consola)
        {
            consola.Escribir("\nIngrese el ID de la solicitud a RECHAZAR: ");

            if (!int.TryParse(consola.LeerTexto(), out int idSolicitud) || idSolicitud <= 0)
            {
                consola.Escribir("[ERROR] Debe ingresar un número entero positivo.\n");
                consola.EsperarTecla();
                return;
            }

            try
            {
                await SolicitudPrestamoServicio.RechazarSolicitudAsync(idSolicitud);

                consola.Escribir($"\n[ÉXITO] La solicitud #{idSolicitud} ha sido RECHAZADA. El cupo del usuario se ha liberado.\n");
            }
            catch (ArgumentException ex)
            {
                consola.Escribir($"\n[REGLA DE NEGOCIO] {ex.Message}\n");
            }
            catch (DbUpdateException)
            {
                consola.Escribir("\n[ERROR BASE DE DATOS] No se pudo actualizar el estado de la solicitud.\n");
            }
            catch (SqlException)
            {
                consola.Escribir("\n[ERROR DE CONEXIÓN] Fallo de comunicación con SQL Server.\n");
            }

            consola.Escribir("Presione cualquier tecla para continuar...\n");
            consola.EsperarTecla();
        }
    }
}
