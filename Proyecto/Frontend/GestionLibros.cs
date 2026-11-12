using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    internal class GestionLibros
    {
        /*static async public Task Menu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("GESTION DE Libros");
                Console.WriteLine("1 Listar | 2 Listar por Autor | 3 Buscar | 4 Alta");
                Console.WriteLine("5 Modificar | 6 Borrar | 0 Salir");
                string opcion = LeerTexto("Opción: ");
                if (opcion == "0") break;
                try
                {
                    switch (opcion)
                    {
                        case "1": await ListarAsync(); break;
                        case "2": await ListarArtistaAsync(); break;
                        case "3": await BuscarAsync(); break;
                        case "4": await InsertarAsync(); break;
                        case "5": await ModificarAsync(); break;
                        case "6": await BorrarAsync(); break;
                        default:
                            Console.WriteLine("Opción no válida.");
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("La operación SQL ha fallado.");
                    Console.WriteLine(ex.Message);
                }
                catch (DbUpdateException)
                {
                    Console.WriteLine(
                    "No se pudieron guardar los cambios.");
                }
            }
        }
        static void Mostrar(Libro e)
        {
            Console.WriteLine($"Id Material: {e.FkmaterialBiblioteca}");
            Console.WriteLine($"Autor: {e.Autor}");
            Console.WriteLine($"ISBN: {e.Isbn}'");
            Console.WriteLine($"Número de Páginas: {e.NumeroPagina}'");
            Console.WriteLine("==============================");
        }

        static string LeerTexto(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine()?.Trim() ?? "";
        }
        static int LeerId()
        {
            if (!int.TryParse(LeerTexto("Id Material: "), out int id))
                throw new ArgumentException("Escribe un entero.");
            return id;
        }

        async static Task ListarAsync()
        {
            var Libros = await LibroServicio.ListarAsync();
            foreach (var e in Libros) Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();

        }
        async static Task ListarArtistaAsync()
        {
            var autor = LeerTexto("Autor: ");

            var Libros = await LibroServicio.ListarAsync();
            Libros = Libros.Where(c => c.Autor.Contains(autor)).ToList();
            foreach (var e in Libros) Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();

        }
        async static Task BuscarAsync()
        {
            var e = await LibroServicio.BuscarAsync(LeerId());

            if (e is null)
            {
                Console.WriteLine("No existe ese Id.");

                Console.WriteLine("Pulsa una tecla para continuar...");
                Console.ReadKey();
            }
            else Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task InsertarAsync()
        {
            var e = new Libro
            {
                FkmaterialBiblioteca = LeerId(),
                Autor = LeerTexto("Autor: "),
                Isbn = LeerTexto("ISBN: "),
                NumeroPagina = (short)(int.TryParse(LeerTexto("Número de Páginas: "), out int pagina) ? pagina : 0)
            };

            int id = await LibroServicio.InsertarAsync(e);
            Console.WriteLine($"Libro creado con Id de Material Biblioteca {id}");

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task ModificarAsync()
        {
            var e = await LibroServicio.BuscarAsync(LeerId());
            if (e is null)
            {
                Console.WriteLine("No existe ese Id.");
                Console.WriteLine("Pulsa una tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Mostrar(e);

            e.Autor = LeerTexto("Nuevo Autor: ");
            e.Isbn = LeerTexto("Nuevo ISBN: ");
            e.NumeroPagina = (short)(int.TryParse(LeerTexto("Nuevo Número de Páginas: "), out int pagina) ? pagina : 0);

            int id = await LibroServicio.ModificarAsync(e);
            Console.WriteLine($"Libro Id {id} modificado.");

            Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task BorrarAsync()
        {
            int id = LeerId();
            var e = await LibroServicio.BuscarAsync(id);
            if (e is null)
            {
                Console.WriteLine("No existe ese Id.");
                Console.WriteLine("Pulsa una tecla para continuar...");
                Console.ReadKey();
                return;
            }
            Mostrar(e);
            if (LeerTexto("¿Borrar este Material de Biblioteca? (S/N): ")
            .ToUpperInvariant() != "S") return;

            bool borrado = await LibroServicio.BorrarAsync(id);

            Console.WriteLine(borrado ? "Libro borrado."
            : "No se borró: ya no existe o tiene relaciones.");
            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }*/

    }
}
