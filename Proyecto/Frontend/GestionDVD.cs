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
    internal class GestionDVD
    {
        /*static async public Task Menu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("GESTION DE DVDS");
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
        static void Mostrar(Dvd e)
        {
            Console.WriteLine($"Id Material: {e.FkmaterialBiblioteca}");
            Console.WriteLine($"Director: {e.NombreDirector}");
            Console.WriteLine($"Clasificación Edad: {e.ClasificacionEdad}");
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
            var DVDs = await DVDServicio.ListarAsync();
            foreach (var e in DVDs) Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();

        }
        async static Task ListarArtistaAsync()
        {
            var director = LeerTexto("Director: ");

            var DVDs = await DVDServicio.ListarAsync();
            DVDs = DVDs.Where(c => c.NombreDirector.Contains(director)).ToList();
            foreach (var e in DVDs) Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();

        }
        async static Task BuscarAsync()
        {
            var e = await DVDServicio.BuscarAsync(LeerId());

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
            var e = new Dvd
            {
                FkmaterialBiblioteca = LeerId(),
                NombreDirector = LeerTexto("Director: "),
                ClasificacionEdad = LeerTexto("Clasificación Edad: "),
                DuracionMinutos = (short)(int.TryParse(LeerTexto("Duración (minutos): "), out int duracion) ? duracion : 0)
            };

            int id = await DVDServicio.InsertarAsync(e);
            Console.WriteLine($"DVD creado con Id de Material Biblioteca {id}");

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task ModificarAsync()
        {
            var e = await DVDServicio.BuscarAsync(LeerId());
            if (e is null)
            {
                Console.WriteLine("No existe ese Id.");
                Console.WriteLine("Pulsa una tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Mostrar(e);

            e.NombreDirector = LeerTexto("Nuevo Director: ");
            e.ClasificacionEdad = LeerTexto("Nueva Clasificación Edad: ");
            e.DuracionMinutos = (short)(int.TryParse(LeerTexto("Nueva Duración (minutos): "), out int duracion) ? duracion : 0);

            int id = await DVDServicio.ModificarAsync(e);
            Console.WriteLine($"DVD Id {id} modificado.");

            Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task BorrarAsync()
        {
            int id = LeerId();
            var e = await DVDServicio.BuscarAsync(id);
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

            bool borrado = await DVDServicio.BorrarAsync(id);

            Console.WriteLine(borrado ? "DVD borrado."
            : "No se borró: ya no existe o tiene relaciones.");
            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }
        */
    }
}
