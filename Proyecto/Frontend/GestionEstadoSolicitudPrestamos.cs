
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    internal class GestionEstadoSolicitudPrestamos
    {
        public static async Task Menu(IConsola consola)
        {
            while (true)
            {
                consola.Limpiar();
                consola.Escribir("GESTION DE ESTADOS DE SOLICITUDES DE PRESTAMO\n");
                consola.Escribir("1 Listar | 2 Buscar | 3 Alta\n");
                consola.Escribir("4 Modificar | 5 Borrar | 0 Salir\n");

                string opcion = LeerTexto(consola, "Opción: ");

                if (opcion == "0") break;

                try
                {
                    switch (opcion)
                    {
                        case "1": await ListarAsync(consola); break;
                        case "2": await BuscarAsync(consola); break;
                        case "3": await InsertarAsync(consola); break;
                        case "4": await ModificarAsync(consola); break;
                        case "5": await BorrarAsync(consola); break;
                        default:
                            consola.Escribir("Opción no válida.\n");
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    consola.Escribir(ex.Message + "\n");
                }
                catch (SqlException ex)
                {
                    consola.Escribir("La operación SQL ha fallado.\n");
                    consola.Escribir(ex.Message + "\n");
                }
                catch (DbUpdateException)
                {
                    consola.Escribir("No se pudieron guardar los cambios.\n");
                }
            }
        }

        static void Mostrar(EstadoSolicitudPrestamo e, IConsola consola)
        {
            consola.Escribir($"Id: {e.IdEstadoSolicitudPrestamo}\n");
            consola.Escribir($"Descripción: {e.NombreEstado}\n");
            consola.Escribir("==============================\n");
        }

        static string LeerTexto(IConsola consola, string mensaje)
        {
            consola.Escribir(mensaje);
            return consola.LeerTexto();
        }

        static int LeerId(IConsola consola)
        {
            if (!int.TryParse(LeerTexto(consola, "Id EstadoSolicitudPrestamos: "), out int id))
                throw new ArgumentException("Escribe un entero.");

            return id;
        }

        static async Task ListarAsync(IConsola consola)
        {
            var estados = await EstadoSolicitudPrestamoServicio.ListarAsync();

            foreach (var e in estados)
                Mostrar(e, consola);

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task BuscarAsync(IConsola consola)
        {
            var e = await EstadoSolicitudPrestamoServicio.BuscarAsync(LeerId(consola));

            if (e is null)
                consola.Escribir("No existe ese Id.\n");
            else
                Mostrar(e, consola);

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task InsertarAsync(IConsola consola)
        {
            var e = new EstadoSolicitudPrestamo
            {
                IdEstadoSolicitudPrestamo = LeerId(consola),
                NombreEstado = LeerTexto(consola, "Descripción: ")
            };

            int id = await EstadoSolicitudPrestamoServicio.InsertarAsync(e);

            consola.Escribir($"EstadoSolicitudPrestamos creado con Id {id}\n");
            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task ModificarAsync(IConsola consola)
        {
            var e = await EstadoSolicitudPrestamoServicio.BuscarAsync(LeerId(consola));

            if (e is null)
            {
                consola.Escribir("No existe ese Id.\n");
                consola.Escribir("Pulsa una tecla para continuar...\n");
                consola.EsperarTecla();
                return;
            }

            Mostrar(e, consola);

            e.NombreEstado = LeerTexto(consola, "Nueva Descripción: ");

            int id = await EstadoSolicitudPrestamoServicio.ModificarAsync(e);

            consola.Escribir($"EstadoSolicitudPrestamos Id {id} modificado.\n");
            Mostrar(e, consola);

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task BorrarAsync(IConsola consola)
        {
            int id = LeerId(consola);
            var e = await EstadoSolicitudPrestamoServicio.BuscarAsync(id);

            if (e is null)
            {
                consola.Escribir("No existe ese Id.\n");
                consola.Escribir("Pulsa una tecla para continuar...\n");
                consola.EsperarTecla();
                return;
            }

            Mostrar(e, consola);

            if (LeerTexto(consola, "¿Borrar este Material de Biblioteca? (S/N): ")
                .ToUpperInvariant() != "S")
                return;

            bool borrado = await EstadoSolicitudPrestamoServicio.BorrarAsync(id);

            consola.Escribir(borrado
                ? "EstadoSolicitudPrestamos borrado.\n"
                : "No se borró: ya no existe o tiene relaciones.\n");

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }
    }
}
