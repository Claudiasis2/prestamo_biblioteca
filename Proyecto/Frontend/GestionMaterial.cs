using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    static class GestionMaterial
    {
        static async public Task Menu()
        {
            while (true)
            {
                //Console.WriteLine($"Usando DB {Configuracion.ReturnDB()}");
                Console.Clear();
                Console.WriteLine("GESTION DE MATERIALES");
                Console.WriteLine("1 Listar | 2 Listar por titulo | 3 Buscar | 4 Alta");
                Console.WriteLine("5 Modificar | 6 Borrar | 0 Salir");
                string opcion = LeerTexto("Opción: ");
                if (opcion == "0") break;
                try
                {
                    switch (opcion)
                    {
                        case "1": await ListarAsync(); break;
                        case "2": await ListarTituloAsync(); break;
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
        static void Mostrar(MaterialBiblioteca e)
        { 
            Console.WriteLine(
            $"{e.IdMaterial} | {e.Titulo}");
            Console.WriteLine($"Año de Publicación: {e.AnioPublicacion}");
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
            var materiales = await MaterialBibliotecaServicio.ListarAsync();
            foreach (var e in materiales) Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();

        }
        async static Task ListarTituloAsync()
        {
            var titulo = LeerTexto("Titulo: ");

            var materiales = await MaterialBibliotecaServicio.ListarAsync();
            materiales = materiales.Where(m => m.Titulo.Contains(titulo)).ToList();
            foreach (var e in materiales) Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();

        }
        async static Task BuscarAsync()
        {
            var e = await MaterialBibliotecaServicio.BuscarAsync(LeerId());

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
            var e = new MaterialBiblioteca
            {

                Titulo = LeerTexto("Titulo: "),
                AnioPublicacion = (short) (int.TryParse(LeerTexto("Año Publicacion: "), out int anioPublicacion) ? anioPublicacion : 0)

            };

            int id = await MaterialBibliotecaEF.InsertarAsync(e);
            Console.WriteLine($"Material Biblioteca creado con Id {id}");

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task ModificarAsync()
        {
            var e = await MaterialBibliotecaServicio.BuscarAsync(LeerId());
            if (e is null)
            {
                Console.WriteLine("No existe ese Id.");
                Console.WriteLine("Pulsa una tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Mostrar(e);

            e.Titulo = LeerTexto("Nuevo Titulo: ");
            e.AnioPublicacion = (short)(int.TryParse(LeerTexto("Nuevo Año Publicacion: "), out int anioPublicacion) ? anioPublicacion : 0);

            int id = await MaterialBibliotecaEF.ModificarAsync(e);
            Console.WriteLine($"Material Biblioteca Id {id} modificado.");

            Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task BorrarAsync()
        {
            int id = LeerId();
            var e = await MaterialBibliotecaServicio.BuscarAsync(id);
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

            bool borrado = await MaterialBibliotecaServicio.BorrarAsync(id);

            Console.WriteLine(borrado ? "Material Biblioteca borrado."
            : "No se borró: ya no existe o tiene relaciones.");
            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }
    }
}