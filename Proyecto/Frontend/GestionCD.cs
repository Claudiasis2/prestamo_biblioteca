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
    internal class GestionCD
    {
        /*
        static async public Task Menu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("GESTION DE CDS");
                Console.WriteLine("1 Listar | 2 Listar por Artista | 3 Buscar | 4 Alta");
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
        
        static void Mostrar(Cd e)
        {
            Console.WriteLine($"Id Material: {e.FkmaterialBiblioteca}");
            Console.WriteLine($"Artista: {e.NombreArtista}");
            Console.WriteLine($"Duración: {e.DuracionMinutos}'");
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
            var cds = await CDServicio.ListarAsync();
            foreach (var e in cds) Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();

        }
        async static Task ListarArtistaAsync()
        {
            var artista = LeerTexto("Artista: ");

            var cds = await CDServicio.ListarAsync();
            cds = cds.Where(c => c.NombreArtista.Contains(artista)).ToList();
            foreach (var e in cds) Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();

        }
        async static Task BuscarAsync()
        {
            var e = await CDServicio.BuscarAsync(LeerId());

            if (e is null)
            {
                Console.WriteLine("No existe ese Id.");

                Console.WriteLine("Pulsa una tecla para continuar...");
                Console.ReadKey();
            }
            else //Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task InsertarAsync()
        {
            var e = new Cd
            {
                FkmaterialBiblioteca = LeerId(),
                NombreArtista = LeerTexto("Artista: "),
                DuracionMinutos = (short)(int.TryParse(LeerTexto("Duración (minutos): "), out int duracion) ? duracion : 0)
            };  

            int id = await CDServicio.InsertarAsync(e);
            Console.WriteLine($"CD creado con Id de Material Biblioteca {id}");

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task ModificarAsync()
        {
            var e = await CDServicio.BuscarAsync(LeerId());
            if (e is null)
            {
                Console.WriteLine("No existe ese Id.");
                Console.WriteLine("Pulsa una tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Mostrar(e);

            e.NombreArtista = LeerTexto("Nuevo Artista: ");
            e.DuracionMinutos = (short)(int.TryParse(LeerTexto("Nueva Duración (minutos): "), out int duracion) ? duracion : 0);

            int id = await CDServicio.ModificarAsync(e);
            Console.WriteLine($"CD Id {id} modificado.");

            Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task BorrarAsync()
        {
            int id = LeerId();
            var e = await CDServicio.BuscarAsync(id);
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

            bool borrado = await CDServicio.BorrarAsync(id);

            Console.WriteLine(borrado ? "CD borrado."
            : "No se borró: ya no existe o tiene relaciones.");
            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }*/

    }
}
