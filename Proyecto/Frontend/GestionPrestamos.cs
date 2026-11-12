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
    internal class GestionPrestamos
    {
        static async public Task Menu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("GESTION DE PRESTAMOS");
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
        static void Mostrar(Prestamo e)
        {
            Console.WriteLine($"Id: {e.IdPrestamo}");
            Console.WriteLine($"Fecha de préstamo: {e.FechaSalida}");
            Console.WriteLine($"Fecha de devolución: {e.FechaLimiteDevolucion}");
            Console.WriteLine($"Fecha de devolución real: {e.FechaDevolucionReal}");
            Console.WriteLine("==============================");
        }

        static string LeerTexto(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine()?.Trim() ?? "";
        }
        static int LeerId()
        {
            if (!int.TryParse(LeerTexto("Id Préstamo: "), out int id))
                throw new ArgumentException("Escribe un entero.");
            return id;
        }

        async static Task ListarAsync()
        {
            var Prestamos = await PrestamoServicio.ListarAsync();
            foreach (var e in Prestamos) Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();

        }
        async static Task BuscarAsync()
        {
            var e = await PrestamoServicio.BuscarAsync(LeerId());

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
            var e = new Prestamo
            {
                FechaSalida = DateTime.Parse(LeerTexto("Fecha de salida (yyyy-MM-dd): ")),
                FechaLimiteDevolucion = DateOnly.Parse(LeerTexto("Fecha de devolución (yyyy-MM-dd): ")),
                FechaDevolucionReal = DateTime.Parse(LeerTexto("Fecha de devolución real (yyyy-MM-dd): ")),
                Fkejemplar = int.Parse(LeerTexto("Id del ejemplar: ")),
                FksolicitudPrestamo = int.Parse(LeerTexto("Id de la solicitud de préstamo: ")),
                Fkempleado = int.Parse(LeerTexto("Id del empleado: ")),
                Fkusuario = int.Parse(LeerTexto("Id del usuario: ")),
                Fkestado = int.Parse(LeerTexto("Id del estado: ")),
            };

            int id = await PrestamoServicio.InsertarAsync(e);
            Console.WriteLine($"Prestamo creado con Id de Material Biblioteca {id}");

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task ModificarAsync()
        {
            var e = await PrestamoServicio.BuscarAsync(LeerId());
            if (e is null)
            {
                Console.WriteLine("No existe ese Id.");
                Console.WriteLine("Pulsa una tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Mostrar(e);

            e.FechaSalida = DateTime.Parse(LeerTexto("Nueva fecha de salida (yyyy-MM-dd): "));
            e.FechaLimiteDevolucion = DateOnly.Parse(LeerTexto("Nueva fecha de devolución (yyyy-MM-dd): "));
            e.FechaDevolucionReal = DateTime.Parse(LeerTexto("Nueva fecha de devolución real (yyyy-MM-dd): "));
            e.Fkejemplar = int.Parse(LeerTexto("Nuevo Id del ejemplar: "));
            e.Fkempleado = int.Parse(LeerTexto("Nuevo Id del empleado: "));
            e.Fkusuario = int.Parse(LeerTexto("Nuevo Id del usuario: "));
            e.Fkestado = int.Parse(LeerTexto("Nuevo Id del estado: "));

            int id = await PrestamoServicio.ModificarAsync(e);
            Console.WriteLine($"Prestamo Id {id} modificado.");

            Mostrar(e);

            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

        async static Task BorrarAsync()
        {
            int id = LeerId();
            var e = await PrestamoServicio.BuscarAsync(id);
            if (e is null)
            {
                Console.WriteLine("No existe ese Id.");
                Console.WriteLine("Pulsa una tecla para continuar...");
                Console.ReadKey();
                return;
            }
            Mostrar(e);
            if (LeerTexto("¿Borrar este Prestamo? (S/N): ")
            .ToUpperInvariant() != "S") return;

            bool borrado = await PrestamoServicio.BorrarAsync(id);

            Console.WriteLine(borrado ? "Prestamo borrado."
            : "No se borró: ya no existe o tiene relaciones.");
            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.ReadKey();
        }

    }
}