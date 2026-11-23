
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Servicios;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    public static class GestionPrestamosAdmin
    {
        public static async Task Menu(IConsola consola)
        {
            bool salir = false;

            while (!salir)
            {
                consola.Limpiar();
                consola.Escribir("==================================================\n");
                consola.Escribir("      GESTIÓN DE PRÉSTAMOS Y DEVOLUCIONES         \n");
                consola.Escribir("==================================================\n");
                consola.Escribir("1. Registrar entrega de material (Crear Préstamo)\n");
                consola.Escribir("2. Registrar devolución de material\n");
                consola.Escribir("3. Ver préstamos activos\n");
                consola.Escribir("0. Volver al menú anterior\n");
                consola.Escribir("--------------------------------------------------\n");
                consola.Escribir("Seleccione una opción: ");

                string opcion = consola.LeerTexto();

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            await RegistrarEntregaAsync(consola);
                            break;
                        case "2":
                            await RegistrarDevolucionAsync(consola);
                            break;
                        case "3":
                            await VerPrestamosActivosAsync(consola);
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

        private static async Task RegistrarEntregaAsync(IConsola consola)
        {
            consola.Limpiar();
            consola.Escribir("==================================================\n");
            consola.Escribir("       REGISTRAR ENTREGA DE MATERIAL FISICO       \n");
            consola.Escribir("==================================================\n");

            var aprobadas = await PrestamoServicio.ListarSolicitudesAprobadasSinEntregarAsync();

            if (!aprobadas.Any())
            {
                consola.Escribir("\n[INFORMACIÓN] No existen solicitudes aprobadas pendientes de entregar.\n");
                consola.Escribir("Presione cualquier tecla para retornar...\n");
                consola.EsperarTecla();
                return;
            }

            consola.Escribir("SOLICITUDES APROBADAS EN ESPERA DE ENTREGA:\n");
            consola.Escribir("----------------------------------------------------------------------------------\n");
            consola.Escribir("ID Sol. | Fecha Solicitud   | Usuario                        | Material\n");
            consola.Escribir("----------------------------------------------------------------------------------\n");

            foreach (var s in aprobadas)
            {
                string usuario = s.FkusuarioNavigation != null
                    ? $"{s.FkusuarioNavigation.NombreUsuario} {s.FkusuarioNavigation.PrimerApellido}".Trim()
                    : $"Usuario #{s.Fkusuario}";

                string titulo = s.FkmaterialBibliotecaNavigation?.Titulo ?? "Material Desconocido";

                consola.Escribir($"{s.IdSolicitud,-7} | {s.FechaSolicitud:dd/MM/yyyy HH:mm} | {usuario,-30} | {titulo}\n");
            }

            consola.Escribir("----------------------------------------------------------------------------------\n");

            consola.Escribir("\nIngrese el ID de la solicitud a entregar: ");

            if (!int.TryParse(consola.LeerTexto(), out int idSolicitud) || idSolicitud <= 0)
            {
                consola.Escribir("[ERROR] Debe ingresar un número entero positivo.\n");
                consola.EsperarTecla();
                return;
            }

            consola.Escribir("Ingrese la cantidad de días de préstamo (Presione ENTER para usar 7 días): ");
            string entradaDias = consola.LeerTexto();

            int diasPrestamo = 7;

            if (!string.IsNullOrEmpty(entradaDias) &&
                int.TryParse(entradaDias, out int d) && d > 0)
            {
                diasPrestamo = d;
            }

            try
            {
                consola.Escribir("\nAsignando ejemplar disponible y registrando préstamo...\n");

                int idPrestamo = await PrestamoServicio.CrearPrestamoAsync(idSolicitud, diasPrestamo);

                consola.Escribir("\n==================================================\n");
                consola.Escribir("        ¡PRÉSTAMO REGISTRADO CON ÉXITO!\n");
                consola.Escribir("==================================================\n");
                consola.Escribir($"Folio de préstamo generado: #{idPrestamo}\n");
                consola.Escribir($"Plazo de devolución esperada: {diasPrestamo} días ({DateTime.Now.AddDays(diasPrestamo):dd/MM/yyyy})\n");
                consola.Escribir("El ejemplar físico fue marcado como PRESTADO en estantería.\n");
            }
            catch (ArgumentException ex)
            {
                consola.Escribir($"\n[REGLA DE NEGOCIO] {ex.Message}\n");
            }
            catch (InvalidOperationException ex)
            {
                consola.Escribir($"\n[DISPONIBILIDAD] {ex.Message}\n");
            }
            catch (DbUpdateException)
            {
                consola.Escribir("\n[ERROR BASE DE DATOS] No se pudo guardar la transacción de préstamo.\n");
            }
            catch (SqlException)
            {
                consola.Escribir("\n[ERROR CONEXIÓN] Fallo de comunicación con SQL Server.\n");
            }

            consola.Escribir("\nPresione cualquier tecla para continuar...\n");
            consola.EsperarTecla();
        }

        private static async Task RegistrarDevolucionAsync(IConsola consola)
        {
            consola.Limpiar();
            consola.Escribir("==================================================\n");
            consola.Escribir("            REGISTRAR DEVOLUCIÓN FÍSICA           \n");
            consola.Escribir("==================================================\n");

            var activos = await PrestamoServicio.ListarPrestamosActivosAsync();

            if (!activos.Any())
            {
                consola.Escribir("\n[INFORMACIÓN] No hay préstamos activos en este momento.\n");
                consola.Escribir("Presione cualquier tecla para retornar...\n");
                consola.EsperarTecla();
                return;
            }

            consola.Escribir("PRÉSTAMOS ACTIVOS EN CIRCULACIÓN:\n");
            consola.Escribir("----------------------------------------------------------------------------------\n");
            consola.Escribir("ID Préstamo | Ejemplar | Usuario                        | F. Límite   | Material\n");
            consola.Escribir("----------------------------------------------------------------------------------\n");

            foreach (var p in activos)
            {
                string usuario = p.FkusuarioNavigation != null
                    ? $"{p.FkusuarioNavigation.NombreUsuario} {p.FkusuarioNavigation.PrimerApellido}".Trim()
                    : $"Usuario #{p.Fkusuario}";

                string titulo = p.FkejemplarNavigation?.FkmaterialBibliotecaNavigation?.Titulo ?? "Material Desconocido";
                int idEjemplar = p.FkejemplarNavigation?.IdEjemplar ?? 0;

                consola.Escribir($"{p.IdPrestamo,-11} | #{idEjemplar,-7} | {usuario,-30} | {p.FechaLimiteDevolucion:dd/MM/yyyy} | {titulo}\n");
            }

            consola.Escribir("----------------------------------------------------------------------------------\n");

            consola.Escribir("\nIngrese el ID del préstamo a DEVOLVER: ");

            if (!int.TryParse(consola.LeerTexto(), out int idPrestamo) || idPrestamo <= 0)
            {
                consola.Escribir("[ERROR] Debe ingresar un número entero positivo.\n");
                consola.EsperarTecla();
                return;
            }

            try
            {
                consola.Escribir("\nProcesando devolución y liberando ejemplar...\n");

                await PrestamoServicio.RegistrarDevolucionAsync(idPrestamo);

                consola.Escribir("\n==================================================\n");
                consola.Escribir("       ¡DEVOLUCIÓN REGISTRADA CON ÉXITO!\n");
                consola.Escribir("==================================================\n");
                consola.Escribir($"Préstamo #{idPrestamo} marcado como DEVUELTO / FINALIZADO.\n");
                consola.Escribir("El ejemplar físico ha retornado al estado DISPONIBLE en estantería.\n");
                consola.Escribir("El cupo del usuario se ha liberado.\n");
            }
            catch (ArgumentException ex)
            {
                consola.Escribir($"\n[REGLA DE NEGOCIO] {ex.Message}\n");
            }
            catch (InvalidOperationException ex)
            {
                consola.Escribir($"\n[ESTADO] {ex.Message}\n");
            }
            catch (DbUpdateException)
            {
                consola.Escribir("\n[ERROR BASE DE DATOS] No se pudo guardar la devolución.\n");
            }
            catch (SqlException)
            {
                consola.Escribir("\n[ERROR CONEXIÓN] Inconveniente de comunicación con SQL Server.\n");
            }

            consola.Escribir("\nPresione cualquier tecla para continuar...\n");
            consola.EsperarTecla();
        }

        private static async Task VerPrestamosActivosAsync(IConsola consola)
        {
            consola.Limpiar();
            consola.Escribir("==================================================\n");
            consola.Escribir("             LISTADO DE PRÉSTAMOS ACTIVOS         \n");
            consola.Escribir("==================================================\n");

            var activos = await PrestamoServicio.ListarPrestamosActivosAsync();

            if (!activos.Any())
            {
                consola.Escribir("\n[INFORMACIÓN] No hay préstamos en circulación actualmente.\n");
            }
            else
            {
                consola.Escribir("ID Préstamo | Ejemplar | Usuario                        | F. Préstamo | F. Límite   | Material\n");
                consola.Escribir("-------------------------------------------------------------------------------------------------\n");

                foreach (var p in activos)
                {
                    string usuario = p.FkusuarioNavigation != null
                        ? $"{p.FkusuarioNavigation.NombreUsuario} {p.FkusuarioNavigation.PrimerApellido}".Trim()
                        : $"Usuario #{p.Fkusuario}";

                    string titulo = p.FkejemplarNavigation?.FkmaterialBibliotecaNavigation?.Titulo ?? "Material Desconocido";
                    int idEjemplar = p.FkejemplarNavigation?.IdEjemplar ?? 0;

                    consola.Escribir($"{p.IdPrestamo,-11} | #{idEjemplar,-7} | {usuario,-30} | {p.FechaSalida:dd/MM/yyyy} | {p.FechaLimiteDevolucion:dd/MM/yyyy} | {titulo}\n");
                }
            }

            consola.Escribir("\nPresione cualquier tecla para retornar al menú...\n");
            consola.EsperarTecla();
        }
    }
}
