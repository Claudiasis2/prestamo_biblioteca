
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    internal class GestionPrestamos
    {
        public static async Task Menu(IConsola consola)
        {
            while (true)
            {
                consola.Limpiar();
                consola.Escribir("GESTION DE PRESTAMOS\n");
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

        static void Mostrar(Prestamo e, IConsola consola)
        {
            consola.Escribir($"Id: {e.IdPrestamo}\n");
            consola.Escribir($"Id del ejemplar: {e.Fkejemplar}\n");
            consola.Escribir($"Id del empleado: {e.Fkempleado}\n");
            consola.Escribir($"Id del estado de la solicitud de préstamo: {e.FkestadoPrestamo}\n");
            consola.Escribir($"Fecha de préstamo: {e.FechaSalida}\n");
            consola.Escribir($"Fecha de devolución: {e.FechaLimiteDevolucion}\n");
            consola.Escribir($"Fecha de devolución real: {e.FechaDevolucionReal}\n");
            consola.Escribir("==============================\n");
        }

        static string LeerTexto(IConsola consola, string mensaje)
        {
            consola.Escribir(mensaje);
            return consola.LeerTexto();
        }

        static int LeerId(IConsola consola)
        {
            if (!int.TryParse(LeerTexto(consola, "Id Préstamo: "), out int id))
                throw new ArgumentException("Escribe un entero.");

            return id;
        }

        static async Task ListarAsync(IConsola consola)
        {
            var prestamos = await PrestamoServicio.ListarAsync();

            foreach (var e in prestamos)
                Mostrar(e, consola);

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task BuscarAsync(IConsola consola)
        {
            var e = await PrestamoServicio.BuscarAsync(LeerId(consola));

            if (e is null)
                consola.Escribir("No existe ese Id.\n");
            else
                Mostrar(e, consola);

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task InsertarAsync(IConsola consola)
        {
            var e = new Prestamo
            {
                FechaSalida = DateTime.Parse(LeerTexto(consola, "Fecha de salida (yyyy-MM-dd): ")),
                FechaLimiteDevolucion = DateOnly.Parse(LeerTexto(consola, "Fecha de devolución (yyyy-MM-dd): ")),
                FechaDevolucionReal = DateTime.Parse(LeerTexto(consola, "Fecha de devolución real (yyyy-MM-dd): ")),
                Fkejemplar = int.Parse(LeerTexto(consola, "Id del ejemplar: ")),
                FksolicitudPrestamo = int.Parse(LeerTexto(consola, "Id de la solicitud de préstamo: ")),
                Fkempleado = int.Parse(LeerTexto(consola, "Id del empleado: ")),
                Fkusuario = int.Parse(LeerTexto(consola, "Id del usuario: ")),
                FkestadoPrestamo = int.Parse(LeerTexto(consola, "Id del estado: "))
            };

            int id = await PrestamoServicio.InsertarAsync(e);

            consola.Escribir($"Prestamo creado con Id de Material Biblioteca {id}\n");
            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task ModificarAsync(IConsola consola)
        {
            var e = await PrestamoServicio.BuscarAsync(LeerId(consola));

            if (e is null)
            {
                consola.Escribir("No existe ese Id.\n");
                consola.Escribir("Pulsa una tecla para continuar...\n");
                consola.EsperarTecla();
                return;
            }

            Mostrar(e, consola);

            e.FechaSalida = DateTime.Parse(LeerTexto(consola, "Nueva fecha de salida (yyyy-MM-dd): "));
            e.FechaLimiteDevolucion = DateOnly.Parse(LeerTexto(consola, "Nueva fecha de devolución (yyyy-MM-dd): "));
            e.FechaDevolucionReal = DateTime.Parse(LeerTexto(consola, "Nueva fecha de devolución real (yyyy-MM-dd): "));
            e.Fkejemplar = int.Parse(LeerTexto(consola, "Nuevo Id del ejemplar: "));
            e.Fkempleado = int.Parse(LeerTexto(consola, "Nuevo Id del empleado: "));
            e.Fkusuario = int.Parse(LeerTexto(consola, "Nuevo Id del usuario: "));
            e.FkestadoPrestamo = int.Parse(LeerTexto(consola, "Nuevo Id del estado: "));

            int id = await PrestamoServicio.ModificarAsync(e);

            consola.Escribir($"Prestamo Id {id} modificado.\n");
            Mostrar(e, consola);

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task BorrarAsync(IConsola consola)
        {
            int id = LeerId(consola);
            var e = await PrestamoServicio.BuscarAsync(id);

            if (e is null)
            {
                consola.Escribir("No existe ese Id.\n");
                consola.Escribir("Pulsa una tecla para continuar...\n");
                consola.EsperarTecla();
                return;
            }

            Mostrar(e, consola);

            if (LeerTexto(consola, "¿Borrar este Prestamo? (S/N): ")
                .ToUpperInvariant() != "S")
                return;

            bool borrado = await PrestamoServicio.BorrarAsync(id);

            consola.Escribir(borrado
                ? "Prestamo borrado.\n"
                : "No se borró: ya no existe o tiene relaciones.\n");

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }
    }
}
