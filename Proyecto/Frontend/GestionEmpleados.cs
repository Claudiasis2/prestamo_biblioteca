using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    internal class GestionEmpleados
    {
        static async public Task Menu()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("GESTION DE EMPLEADO");
                Console.WriteLine("1 Listar");
                Console.WriteLine("2 Buscar");
                Console.WriteLine("3 Alta");
                Console.WriteLine("4 Modificar");
                Console.WriteLine("5 Borrar");
                Console.WriteLine("0 Salir");
                string opcion = LeerTexto("Opción: ");
                if (opcion == "0") break;
                try
                {
                    switch (opcion)
                    {
                        case "1":
                            await ListarEmpleadoAsync();
                            break;
                        case "2":
                            await BuscarEmpleadoAsync();
                            break;
                        case "3":
                            await InsertarEmpleadoAsync();
                            break;
                        case "4":
                            await ModificarEmpleadoAsync();
                            break;
                        case "5":
                            await BorrarEmpleadoAsync();
                            break;
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

        public async static Task BorrarEmpleadoAsync()
        {
            Console.Clear();
            Console.WriteLine("BORRAR EMPLEADO");
            var lista = await EmpleadoServicio.ListarAsync();
            foreach (var l in lista)
                Mostrar(l);

            if (!int.TryParse(LeerTexto("Introduce el Id a eliminar: "), out int id))
                throw new ArgumentException("Escribe un entero.");
            var empleado = await EmpleadoServicio.BuscarAsync(id);
            if (empleado is null || empleado.Count > 1)
            {
                Console.WriteLine("No existe ese Id.");
                return;
            }

            foreach (var l in empleado)
                Mostrar(l);
            if (LeerTexto("\n¿Borrar este empleado? (S/N): ").ToUpperInvariant() != "S")
                return;
            bool borrado = await EmpleadoServicio.BorrarAsync(id);
            Console.WriteLine(borrado ? "Empleado borrado." : "No se borró: ya no existe o tiene relaciones.");
            SalirPantalla();
        }

        static string LeerTexto(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine()?.Trim() ?? "";
        }

        public async static Task InsertarEmpleadoAsync()
        {

            Console.Clear();
            Console.WriteLine("ALTA DE EMPLEADO");
            Console.WriteLine("Los datos con * son obligatorios.\n\n");
            var e = new Empleado
            {

                Email = LeerTexto("Inserta el correo: "),
                NombreEmpleado = LeerTexto("Inserta el nombre: "),
                PrimerApellido = LeerTexto("Inserta el primer apellido: "),
                SegundoApellido = LeerTexto("Inserta el segundo apellido: ")
            };
            if (e.Email == "") e.Email = null;
            if (e.NombreEmpleado == "") e.NombreEmpleado = null;
            if (e.PrimerApellido == "") e.PrimerApellido = null;
            if (e.SegundoApellido == "") e.SegundoApellido = null;
            int id = await EmpleadoServicio.InsertarAsync(e);
            Console.WriteLine("Empleado creado con éxito.\n");
            var u = await EmpleadoEF.BuscarAsync(id);

            SalirPantalla();
        }

        public static async Task ListarEmpleadoAsync()
        {
            Console.Clear();
            Console.WriteLine("LISTADO DE EMPLEADOS");
            var lista = await EmpleadoServicio.ListarAsync();
            foreach (var u in lista)
                Mostrar(u);

            SalirPantalla();
        }

        public static async Task BuscarEmpleadoAsync()
        {
            Console.Clear();
            Console.WriteLine("BUSCAR EMPLEADO");
            var (email, nombre) = FormularioBusqueda();
            var listaEmpleados = await EmpleadoServicio.BuscarAsync(0, email, nombre);

            if (listaEmpleados is null || !listaEmpleados.Any())
                Console.WriteLine("No se han encontrado coincidencias.");
            else
                foreach (var e in listaEmpleados)
                    Mostrar(e);

            SalirPantalla();
        }

        public async static Task ModificarEmpleadoAsync()
        {
            Console.Clear();
            Console.WriteLine("MODIFICAR DATOS EMPLEADO");
            var (email, nombre) = FormularioBusqueda();
            var listaEmpleados = await EmpleadoServicio.BuscarAsync(0, email, nombre);
            if (listaEmpleados is null || !listaEmpleados.Any())
            {
                Console.WriteLine("No se han encontrado coincidencias.");
                SalirPantalla();
                return;
            }
            if (listaEmpleados.Count > 1)
            {
                foreach (var e in listaEmpleados)
                    Mostrar(e);
                Console.WriteLine("Mas de una coincidencia.");
                SalirPantalla();
                return;
            }

            if (listaEmpleados.Count == 1)
            {
                var ok = await EmpleadoServicio.ModificarAsync(FormularioCambios(listaEmpleados[0]));
                return;
            }


        }

        public static void Mostrar(Empleado e)
        {
            Console.WriteLine($"{e.IdEmpleado} | {e.Email} {e.NombreEmpleado} {e.PrimerApellido} {e.SegundoApellido}");
        }

        public static void SalirPantalla()
        {
            Console.WriteLine("\nPulsa una tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }

        static (string bEmail, string bNombre) FormularioBusqueda()
        {
            Console.WriteLine("FORMULARIO DE BÚSQUEDA");
            Console.WriteLine("Introduce los datos que buscas. Deja en blanco los valores que desconozcas.\n");
            string bEmail = LeerTexto("Introduce el email: ");
            string bNombre = LeerTexto("Introduce el Nombre: ");

            return (bEmail, bNombre);
        }

        public static Empleado FormularioCambios(Empleado u)
        {
            Console.Clear();
            Console.WriteLine("DATOS EMPLEADO");
            Console.WriteLine("Los datos con * son obligatorios.\n\n");
            var e = new Empleado
            {
                IdEmpleado = u.IdEmpleado,
                Email = LeerTexto($"Inserta el correo ({u.Email}): "),
                NombreEmpleado = LeerTexto($"Inserta el nombre ({u.NombreEmpleado}): "),
                PrimerApellido = LeerTexto($"Inserta el primer apellido ({u.PrimerApellido}): "),
                SegundoApellido = LeerTexto($"Inserta el segundo apellido ({u.SegundoApellido}): ")
            };
            if (e.Email == "") e.Email = null;
            if (e.NombreEmpleado == "") e.NombreEmpleado = null;
            if (e.PrimerApellido == "") e.PrimerApellido = null;
            if (e.SegundoApellido == "") e.SegundoApellido = null;

            return e;
        }
    }
}
