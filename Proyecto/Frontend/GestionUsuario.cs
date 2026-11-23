
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;
using ProyectoConjuntoMaterialPrestamo.Frontend;

static class GestionUsuario
{
    public static async Task Menu(IConsola consola)
    {
        consola.Limpiar();

        while (true)
        {
            consola.Escribir("GESTION DE USUARIOS\n");
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
                        await ListarUsuarioAsync(consola);
                        break;

                    case "2":
                        await BuscarUsuarioAsync(consola);
                        break;

                    case "3":
                        await InsertarUsuarioAsync(consola);
                        break;

                    case "4":
                        await ModificarUsuarioAsync(consola);
                        break;

                    case "5":
                        await BorrarUsuarioAsync(consola);
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

    private static string LeerTexto(IConsola consola, string mensaje)
    {
        consola.Escribir(mensaje);
        return consola.LeerTexto();
    }

    private static async Task InsertarUsuarioAsync(IConsola consola)
    {
        consola.Limpiar();
        consola.Escribir("ALTA DE USUARIOS\n");
        consola.Escribir("Los datos con * son obligatorios.\n\n");

        var e = new Usuario
        {
            Email = LeerTexto(consola, "Inserta el correo *: "),
            NombreUsuario = LeerTexto(consola, "Inserta el nombre *: "),
            PrimerApellido = LeerTexto(consola, "Inserta el primer apellido *: "),
            SegundoApellido = LeerTexto(consola, "Inserta el segundo apellido: "),
            Password = LeerTexto(consola, "Inserta el password *: "),
            Activo = true
        };

        if (e.SegundoApellido == "") e.SegundoApellido = null;

        int id = await UsuarioServicio.InsertarAsync(e);

        consola.Escribir("Usuario creado con éxito.\n");

        var u = await UsuarioEF.BuscarAsync(id);

        SalirPantalla(consola);
    }

    private static async Task ListarUsuarioAsync(IConsola consola)
    {
        consola.Limpiar();
        consola.Escribir("LISTADO DE USUARIOS\n");

        var lista = await UsuarioServicio.ListarAsync();

        foreach (var u in lista)
            Mostrar(u, consola);

        SalirPantalla(consola);
    }

    private static async Task ModificarUsuarioAsync(IConsola consola)
    {
        consola.Limpiar();
        consola.Escribir("MODIFICAR DATOS USUARIO\n");

        var (email, nombre) = FormularioBusqueda(consola);

        var listaUsuarios = await UsuarioServicio.BuscarAsync(0, email, nombre);

        if (listaUsuarios is null || listaUsuarios.Count == 0)
        {
            consola.Escribir("No se han encontrado coincidencias.\n");
            SalirPantalla(consola);
            return;
        }

        if (listaUsuarios.Count > 1)
        {
            foreach (var u in listaUsuarios)
                Mostrar(u, consola);

            consola.Escribir("Mas de una coincidencia.\n");
            SalirPantalla(consola);
            return;
        }

        if (listaUsuarios.Count == 1)
        {
            var ok = await UsuarioServicio.ModificarAsync(
                FormularioCambios(listaUsuarios[0], consola));
            return;
        }
    }

    private static async Task BorrarUsuarioAsync(IConsola consola)
    {
        consola.Limpiar();
        consola.Escribir("BORRAR USUARIO\n");

        var lista = await UsuarioServicio.ListarAsync();

        foreach (var l in lista)
            Mostrar(l, consola);

        if (!int.TryParse(
            LeerTexto(consola, "Introduce el Id a eliminar: "),
            out int id))
        {
            throw new ArgumentException("Escribe un entero.");
        }

        var usuario = await UsuarioServicio.BuscarAsync(id);

        if (usuario is null || usuario.Count > 1)
        {
            consola.Escribir("No existe ese Id.\n");
            return;
        }

        foreach (var l in usuario)
            Mostrar(l, consola);

        if (LeerTexto(consola, "\n¿Borrar este empleado? (S/N): ")
            .ToUpperInvariant() != "S")
            return;

        bool borrado = await UsuarioServicio.BorrarAsync(id);

        consola.Escribir(
            borrado
                ? "Empleado borrado.\n"
                : "No se borró: ya no existe o tiene relaciones.\n");

        SalirPantalla(consola);
    }

    public static async Task BuscarUsuarioAsync(IConsola consola)
    {
        consola.Limpiar();
        consola.Escribir("BUSCAR USUARIO\n");

        var (email, nombre) = FormularioBusqueda(consola);

        var listaUsuarios = await UsuarioServicio.BuscarAsync(0, email, nombre);

        if (listaUsuarios is null || !listaUsuarios.Any())
        {
            consola.Escribir("No se han encontrado coincidencias.\n");
        }
        else
        {
            foreach (var u in listaUsuarios)
                Mostrar(u, consola);
        }

        SalirPantalla(consola);
    }

    public static void Mostrar(Usuario u, IConsola consola)
    {
        var estado = u.Activo ? "Activo" : "Desactivado";

        consola.Escribir(
            $"{u.IdUsuario} | {u.Email} {u.NombreUsuario} {u.PrimerApellido} {u.SegundoApellido} {estado}\n");
    }

    public static void SalirPantalla(IConsola consola)
    {
        consola.Escribir("\nPulsa una tecla para continuar...\n");
        consola.EsperarTecla();
        consola.Limpiar();
    }

    private static (string bEmail, string bNombre) FormularioBusqueda(
        IConsola consola)
    {
        consola.Escribir("FORMULARIO DE BÚSQUEDA\n");
        consola.Escribir("Introduce los datos que buscas. Deja en blanco los valores que desconozcas.\n");

        string bEmail = LeerTexto(consola, "Introduce el email: ");
        string bNombre = LeerTexto(consola, "Introduce el Nombre: ");

        return (bEmail, bNombre);
    }

    public static Usuario FormularioCambios(Usuario u, IConsola consola)
    {
        consola.Limpiar();
        consola.Escribir("DATOS USUARIO\n");
        consola.Escribir("Los datos con * son obligatorios.\n\n");

        var e = new Usuario
        {
            IdUsuario = u.IdUsuario,
            Email = LeerTexto(consola, $"Inserta el correo * ({u.Email}): "),
            NombreUsuario = LeerTexto(consola, $"Inserta el nombre * ({u.NombreUsuario}): "),
            PrimerApellido = LeerTexto(consola, $"Inserta el primer apellido * ({u.PrimerApellido}): "),
            SegundoApellido = LeerTexto(consola, $"Inserta el segundo apellido ({u.SegundoApellido}): "),
            Password = u.Password,
            Activo = u.Activo
        };

        if (e.SegundoApellido == "") e.SegundoApellido = null;

        return e;
    }
}
