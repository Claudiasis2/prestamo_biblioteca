
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    static class GestionMaterial
    {
        public static async Task Menu(IConsola consola)
        {
            while (true)
            {
                consola.Limpiar();
                consola.Escribir("GESTION DE MATERIALES\n");
                consola.Escribir("1 Listar | 2 Listar por titulo | 3 Buscar | 4 Alta\n");
                consola.Escribir("5 Modificar | 6 Borrar | 0 Salir\n");

                string opcion = LeerTexto(consola, "Opción: ");

                if (opcion == "0") break;

                try
                {
                    switch (opcion)
                    {
                        case "1": await ListarAsync(consola); break;
                        case "2": await ListarTituloAsync(consola); break;
                        case "3": await BuscarAsync(consola); break;
                        case "4": await InsertarAsync(consola); break;
                        case "5": await ModificarAsync(consola); break;
                        case "6": await BorrarAsync(consola); break;
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

        static void Mostrar(MaterialBiblioteca e, IConsola consola)
        {
            consola.Escribir($"{e.IdMaterial} | {e.Titulo}\n");
            consola.Escribir($"Año de Publicación: {e.AnioPublicacion}\n");
            consola.Escribir("==============================\n");
        }

        static string LeerTexto(IConsola consola, string mensaje)
        {
            consola.Escribir(mensaje);
            return consola.LeerTexto();
        }

        static int LeerId(IConsola consola)
        {
            if (!int.TryParse(LeerTexto(consola, "Id Material: "), out int id))
                throw new ArgumentException("Escribe un entero.");

            return id;
        }

        static async Task ListarAsync(IConsola consola)
        {
            var materiales = await MaterialBibliotecaServicio.ListarAsync();

            foreach (var e in materiales)
                Mostrar(e, consola);

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task ListarTituloAsync(IConsola consola)
        {
            var titulo = LeerTexto(consola, "Titulo: ");

            var materiales = await MaterialBibliotecaServicio.ListarAsync();
            materiales = materiales.Where(m => m.Titulo.Contains(titulo)).ToList();

            foreach (var e in materiales)
                Mostrar(e, consola);

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task BuscarAsync(IConsola consola)
        {
            var e = await MaterialBibliotecaServicio.BuscarAsync(LeerId(consola));

            if (e is null)
                consola.Escribir("No existe ese Id.\n");
            else
                Mostrar(e, consola);

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task InsertarAsync(IConsola consola)
        {
            var e = new MaterialBiblioteca
            {
                Titulo = LeerTexto(consola, "Titulo: "),
                AnioPublicacion = (short)(
                    int.TryParse(LeerTexto(consola, "Año Publicacion: "), out int anioPublicacion)
                    ? anioPublicacion
                    : 0)
            };

            int id = await MaterialBibliotecaEF.InsertarAsync(e);

            consola.Escribir($"Material Biblioteca creado con Id {id}\n");
            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task ModificarAsync(IConsola consola)
        {
            var e = await MaterialBibliotecaServicio.BuscarAsync(LeerId(consola));

            if (e is null)
            {
                consola.Escribir("No existe ese Id.\n");
                consola.Escribir("Pulsa una tecla para continuar...\n");
                consola.EsperarTecla();
                return;
            }

            Mostrar(e, consola);

            e.Titulo = LeerTexto(consola, "Nuevo Titulo: ");
            e.AnioPublicacion = (short)(
                int.TryParse(LeerTexto(consola, "Nuevo Año Publicacion: "), out int anioPublicacion)
                ? anioPublicacion
                : 0);

            int id = await MaterialBibliotecaEF.ModificarAsync(e);

            consola.Escribir($"Material Biblioteca Id {id} modificado.\n");
            Mostrar(e, consola);

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }

        static async Task BorrarAsync(IConsola consola)
        {
            int id = LeerId(consola);
            var e = await MaterialBibliotecaServicio.BuscarAsync(id);

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

            bool borrado = await MaterialBibliotecaServicio.BorrarAsync(id);

            consola.Escribir(borrado
                ? "Material Biblioteca borrado.\n"
                : "No se borró: ya no existe o tiene relaciones.\n");

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();
        }
    }
}
