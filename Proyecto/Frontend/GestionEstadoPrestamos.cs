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
    internal class GestionEstadoPrestamos
    {
        static async public Task Menu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("GESTION DE EstadoPrestamos");
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
        static void Mostrar(EstadoPrestamo e)
        {
            Console.WriteLine($"Id: {e.IdEstadoPrestamo}");
            Console.WriteLine($"Descripción: {e.NombreEstado}");
            Console.WriteLine("==============================");
        }

        static string LeerTexto(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine()?.Trim() ?? "";
        }
        static int LeerId()
        {
            if (!int.TryParse(LeerTexto("Id EstadoPrestamo: "), out int id))
                throw new ArgumentException("Escribe un entero.");
            return id;
        }

        async static Task ListarAsync()
        {
            var EstadoPrestamos = await EstadoPrestamoServicio.ListarAsync();
            foreach (var e in EstadoPrestamos) Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();

        }
        async static Task BuscarAsync()
        {
            var e = await EstadoPrestamoServicio.BuscarAsync(LeerId());

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
            var e = new EstadoPrestamo
            {
                IdEstadoPrestamo = LeerId(),
                NombreEstado = LeerTexto("Descripción: "),
            };

            int id = await EstadoPrestamoServicio.InsertarAsync(e);
            Console.WriteLine($"EstadoPrestamo creado con Id {id}");

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task ModificarAsync()
        {
            var e = await EstadoPrestamoServicio.BuscarAsync(LeerId());
            if (e is null)
            {
                Console.WriteLine("No existe ese Id.");
                Console.WriteLine("Pulsa una tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Mostrar(e);

            e.NombreEstado = LeerTexto("Nueva Descripción: ");

            int id = await EstadoPrestamoServicio.ModificarAsync(e);
            Console.WriteLine($"EstadoPrestamo Id {id} modificado.");

            Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task BorrarAsync()
        {
            int id = LeerId();
            var e = await EstadoPrestamoServicio.BuscarAsync(id);
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

            bool borrado = await EstadoPrestamoServicio.BorrarAsync(id);

            Console.WriteLine(borrado ? "EstadoPrestamo borrado."
            : "No se borró: ya no existe o tiene relaciones.");
            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

    }
}