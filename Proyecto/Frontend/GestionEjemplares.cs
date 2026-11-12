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
    internal class GestionEjemplares
    {
        static async public Task Menu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("GESTION DE EJEMPLARES");
                Console.WriteLine("1 Listar | 2 Buscar | 3 Alta");
                Console.WriteLine("4 Modificar | 5 Borrar | 0 Salir");
                string opcion = LeerTexto("Opción: ");
                if (opcion == "0") break;
                try
                {
                    switch (opcion)
                    {
                        case "1": await ListarAsync(); break;
                        case "2": await BuscarAsync(); break;
                        case "3": await InsertarAsync(); break;
                        case "4": await ModificarAsync(); break;
                        case "5": await BorrarAsync(); break;
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
        static void Mostrar(Ejemplar e)
        {
            Console.WriteLine($"Id: {e.IdEjemplar}");
            Console.WriteLine($"Id Material Biblioteca: {e.FkmaterialBiblioteca}");
            Console.WriteLine($"Ubicación en estantería: {e.UbicacionEstanteria}");
            Console.WriteLine($"Id Estado: {e.Fkestado}");
            Console.WriteLine("==============================");
        }

        static string LeerTexto(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine()?.Trim() ?? "";
        }
        static int LeerId()
        {
            if (!int.TryParse(LeerTexto("Id Ejemplar: "), out int id))
                throw new ArgumentException("Escribe un entero.");
            return id;
        }

        async static Task ListarAsync()
        {
            var Ejemplares = await EjemplarServicio.ListarAsync();
            foreach (var e in Ejemplares) Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();

        }
        async static Task BuscarAsync()
        {
            var e = await EjemplarServicio.BuscarAsync(LeerId());

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
            var e = new Ejemplar
            {
                IdEjemplar = LeerId(),
                UbicacionEstanteria = LeerTexto("Ubicación en estantería: "),
                Fkestado = LeerId()
            };

            int id = await EjemplarServicio.InsertarAsync(e);
            Console.WriteLine($"Ejemplar creado con Id {id}");

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task ModificarAsync()
        {
            var e = await EjemplarServicio.BuscarAsync(LeerId());
            if (e is null)
            {
                Console.WriteLine("No existe ese Id.");
                Console.WriteLine("Pulsa una tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Mostrar(e);

            e.UbicacionEstanteria = LeerTexto("Nueva Ubicación en estantería: ");
            e.Fkestado = LeerId();

            int id = await EjemplarServicio.ModificarAsync(e);
            Console.WriteLine($"Ejemplar Id {id} modificado.");

            Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task BorrarAsync()
        {
            int id = LeerId();
            var e = await EjemplarServicio.BuscarAsync(id);
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

            bool borrado = await EjemplarServicio.BorrarAsync(id);

            Console.WriteLine(borrado ? "Ejemplar borrado."
            : "No se borró: ya no existe o tiene relaciones.");
            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

    }
}