
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    internal class GestionEjemplares
    {
        public static async Task Menu(IConsola consola)
        {
            while (true)
            {
                consola.Limpiar();
                consola.Escribir("GESTION DE EJEMPLARES\n");
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

        static void Mostrar(Ejemplar e, IConsola consola)
        {
            consola.Escribir($"Id: {e.IdEjemplar}\n");
            consola.Escribir($"Id Material Biblioteca: {e.FkmaterialBiblioteca}\n");
            consola.Escribir($"Ubicación en estantería: {e.UbicacionEstanteria}\n");
            consola.Escribir($"Id Estado: {e.Fkestado}\n");
            consola.Escribir("==============================\n");
        }

        static string LeerTexto(IConsola consola, string mensaje)
        {
            consola.Escribir(mensaje);
            return consola.LeerTexto();
        }

        static int LeerId(IConsola consola)
        {
            if (!int.TryParse(LeerTexto(consola, "Id Ejemplar: "), out int id))
                throw new ArgumentException("Escribe un entero.");

            return id;
        }

        static async Task ListarAsync(IConsola consola)
        {
            var ejemplares = await EjemplarServicio.ListarAsync();

            foreach (var e in ejemplares)
                Mostrar(e, consola);

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task BuscarAsync(IConsola consola)
        {
            var e = await EjemplarServicio.BuscarAsync(LeerId(consola));

            if (e is null)
            {
                consola.Escribir("No existe ese Id.\n");
            }
            else
            {
                Mostrar(e, consola);
            }

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task InsertarAsync(IConsola consola)
        {
            var e = new Ejemplar
            {
                IdEjemplar = LeerId(consola),
                UbicacionEstanteria = LeerTexto(consola, "Ubicación en estantería: "),
                Fkestado = LeerId(consola)
            };

            int id = await EjemplarServicio.InsertarAsync(e);
            consola.Escribir($"Ejemplar creado con Id {id}\n");

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task ModificarAsync(IConsola consola)
        {
            var e = await EjemplarServicio.BuscarAsync(LeerId(consola));

            if (e is null)
            {
                consola.Escribir("No existe ese Id.\n");
                consola.Escribir("Pulsa una tecla para continuar...\n");
                consola.EsperarTecla();
                return;
            }

            Mostrar(e, consola);

            e.UbicacionEstanteria = LeerTexto(consola, "Nueva Ubicación en estantería: ");
            e.Fkestado = LeerId(consola);

            int id = await EjemplarServicio.ModificarAsync(e);
            consola.Escribir($"Ejemplar Id {id} modificado.\n");

            Mostrar(e, consola);

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task BorrarAsync(IConsola consola)
        {
            int id = LeerId(consola);
            var e = await EjemplarServicio.BuscarAsync(id);

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

            bool borrado = await EjemplarServicio.BorrarAsync(id);

            consola.Escribir(borrado
                ? "Ejemplar borrado.\n"
                : "No se borró: ya no existe o tiene relaciones.\n");

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }
    }
}
