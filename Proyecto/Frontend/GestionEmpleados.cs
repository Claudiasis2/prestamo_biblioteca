
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;

namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    internal class GestionEmpleados
    {
        public static async Task Menu(IConsola consola)
        {
            consola.Limpiar();

            while (true)
            {
                consola.Escribir("GESTION DE EMPLEADO\n");
                consola.Escribir("1 Listar\n");
                consola.Escribir("2 Buscar\n");
                consola.Escribir("3 Alta\n");
                consola.Escribir("4 Modificar\n");
                consola.Escribir("5 Borrar\n");
                consola.Escribir("0 Salir\n");

                string opcion = LeerTexto(consola, "Opción: ");

                if (opcion == "0") break;

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            await ListarEmpleadoAsync(consola);
                            break;
                        case "2":
                            await BuscarEmpleadoAsync(consola);
                            break;
                        case "3":
                            await InsertarEmpleadoAsync(consola);
                            break;
                        case "4":
                            await ModificarEmpleadoAsync(consola);
                            break;
                        case "5":
                            await BorrarEmpleadoAsync(consola);
                            break;
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

        public static async Task BorrarEmpleadoAsync(IConsola consola)
        {
            consola.Limpiar();
            consola.Escribir("BORRAR EMPLEADO\n");

            var lista = await EmpleadoServicio.ListarAsync();

            foreach (var l in lista)
                Mostrar(l, consola);

            if (!int.TryParse(LeerTexto(consola, "Introduce el Id a eliminar: "), out int id))
                throw new ArgumentException("Escribe un entero.");

            var empleado = await EmpleadoServicio.BuscarAsync(id);

            if (empleado is null || empleado.Count > 1)
            {
                consola.Escribir("No existe ese Id.\n");
                return;
            }

            foreach (var l in empleado)
                Mostrar(l, consola);

            if (LeerTexto(consola, "\n¿Borrar este empleado? (S/N): ").ToUpperInvariant() != "S")
                return;

            bool borrado = await EmpleadoServicio.BorrarAsync(id);

            consola.Escribir(borrado
                ? "Empleado borrado.\n"
                : "No se borró: ya no existe o tiene relaciones.\n");

            SalirPantalla(consola);
        }

        static string LeerTexto(IConsola consola, string mensaje)
        {
            consola.Escribir(mensaje);
            return consola.LeerTexto();
        }

        public static async Task InsertarEmpleadoAsync(IConsola consola)
        {
            consola.Limpiar();
            consola.Escribir("ALTA DE EMPLEADO\n");
            consola.Escribir("Los datos con * son obligatorios.\n\n");

            var e = new Empleado
            {
                Email = LeerTexto(consola, "Inserta el correo: "),
                NombreEmpleado = LeerTexto(consola, "Inserta el nombre: "),
                PrimerApellido = LeerTexto(consola, "Inserta el primer apellido: "),
                SegundoApellido = LeerTexto(consola, "Inserta el segundo apellido: ")
            };

            if (e.Email == "") e.Email = null;
            if (e.NombreEmpleado == "") e.NombreEmpleado = null;
            if (e.PrimerApellido == "") e.PrimerApellido = null;
            if (e.SegundoApellido == "") e.SegundoApellido = null;

            int id = await EmpleadoServicio.InsertarAsync(e);

            consola.Escribir("Empleado creado con éxito.\n");

            var u = await EmpleadoEF.BuscarAsync(id);

            SalirPantalla(consola);
        }

        public static async Task ListarEmpleadoAsync(IConsola consola)
        {
            consola.Limpiar();
            consola.Escribir("LISTADO DE EMPLEADOS\n");

            var lista = await EmpleadoServicio.ListarAsync();

            foreach (var u in lista)
                Mostrar(u, consola);

            SalirPantalla(consola);
        }

        public static async Task BuscarEmpleadoAsync(IConsola consola)
        {
            consola.Limpiar();
            consola.Escribir("BUSCAR EMPLEADO\n");

            var (email, nombre) = FormularioBusqueda(consola);

            var listaEmpleados = await EmpleadoServicio.BuscarAsync(0, email, nombre);

            if (listaEmpleados is null || !listaEmpleados.Any())
            {
                consola.Escribir("No se han encontrado coincidencias.\n");
            }
            else
            {
                foreach (var e in listaEmpleados)
                    Mostrar(e, consola);
            }

            SalirPantalla(consola);
        }

        public static async Task ModificarEmpleadoAsync(IConsola consola)
        {
            consola.Limpiar();
            consola.Escribir("MODIFICAR DATOS EMPLEADO\n");

            var (email, nombre) = FormularioBusqueda(consola);

            var listaEmpleados = await EmpleadoServicio.BuscarAsync(0, email, nombre);

            if (listaEmpleados is null || !listaEmpleados.Any())
            {
                consola.Escribir("No se han encontrado coincidencias.\n");
                SalirPantalla(consola);
                return;
            }

            if (listaEmpleados.Count > 1)
            {
                foreach (var e in listaEmpleados)
                    Mostrar(e, consola);

                consola.Escribir("Mas de una coincidencia.\n");
                SalirPantalla(consola);
                return;
            }

            if (listaEmpleados.Count == 1)
            {
                var ok = await EmpleadoServicio.ModificarAsync(
                    FormularioCambios(listaEmpleados[0], consola));
                return;
            }
        }

        public static void Mostrar(Empleado e, IConsola consola)
        {
            consola.Escribir(
                $"{e.IdEmpleado} | {e.Email} {e.NombreEmpleado} {e.PrimerApellido} {e.SegundoApellido}\n");
        }

        public static void SalirPantalla(IConsola consola)
        {
            consola.Escribir("\nPulsa una tecla para continuar...\n");
            consola.EsperarTecla();
            consola.Limpiar();
        }

        static (string bEmail, string bNombre) FormularioBusqueda(IConsola consola)
        {
            consola.Escribir("FORMULARIO DE BÚSQUEDA\n");
            consola.Escribir("Introduce los datos que buscas. Deja en blanco los valores que desconozcas.\n\n");

            string bEmail = LeerTexto(consola, "Introduce el email: ");
            string bNombre = LeerTexto(consola, "Introduce el Nombre: ");

            return (bEmail, bNombre);
        }

        public static Empleado FormularioCambios(Empleado u, IConsola consola)
        {
            consola.Limpiar();
            consola.Escribir("DATOS EMPLEADO\n");
            consola.Escribir("Los datos con * son obligatorios.\n\n");

            var e = new Empleado
            {
                IdEmpleado = u.IdEmpleado,
                Email = LeerTexto(consola, $"Inserta el correo ({u.Email}): "),
                NombreEmpleado = LeerTexto(consola, $"Inserta el nombre ({u.NombreEmpleado}): "),
                PrimerApellido = LeerTexto(consola, $"Inserta el primer apellido ({u.PrimerApellido}): "),
                SegundoApellido = LeerTexto(consola, $"Inserta el segundo apellido ({u.SegundoApellido}): ")
            };

            if (e.Email == "") e.Email = null;
            if (e.NombreEmpleado == "") e.NombreEmpleado = null;
            if (e.PrimerApellido == "") e.PrimerApellido = null;
            if (e.SegundoApellido == "") e.SegundoApellido = null;

            return e;
        }
    }
}
